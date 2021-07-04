using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BablTest
{
    [SetUpFixture]
    public class BablHandler
    {
        [OneTimeSetUp]
        public void Init() => 
            BablInit();

        [OneTimeTearDown]
        public void Exit() =>
            BablExit();

        public unsafe static string HexPrint(IntPtr ptr, int size, int lineWidth)
        {
            var bytes = new ReadOnlySpan<byte>(ptr.ToPointer(), size);
            var sb = new StringBuilder();

            var sizeChars = size.ToString("X").Length;
            var bytesPerLine = (lineWidth - (sizeChars + 5)) / 4;
            if (bytesPerLine < 1)
                throw new ArgumentException("Width is too small", nameof(lineWidth));

            for (var i = 0; i < size; i+= bytesPerLine)
            {
                var strBytes = new StringBuilder(lineWidth+1)
                    .Append(i.ToString("X")
                             .PadLeft(sizeChars, '0'))
                    .Append("   ");

                var strChars = new StringBuilder(bytesPerLine);
                                
                for(var j = 0; j < bytesPerLine && j + i < size; j++)
                {
                    strBytes.Append(bytes[i+j].ToString("X")
                                        .PadLeft(2, '0'))
                        .Append(' ');

                    var c = (char)bytes[i + j];
                    strChars.Append(c == ' ' || !char.IsControl(c)
                        ? c
                        : '?');
                }
                sb.Append(strBytes)
                  .Append(' ', lineWidth - (strBytes.Length + strChars.Length))
                  .Append(strChars)
                  .AppendLine();
            }

            return sb.ToString();
        }

        [DllImport("libbabl-0.1-0.dll", EntryPoint = "babl_init")]
        internal extern static void BablInit();

        [DllImport("libbabl-0.1-0.dll", EntryPoint = "babl_exit")]
        internal extern static void BablExit();

        [DllImport("libbabl-0.1-0.dll", EntryPoint = "babl_malloc")]
        internal extern static IntPtr BablAllocate(nint size);

        [DllImport("libbabl-0.1-0.dll", EntryPoint = "babl_free")]
        internal extern static void BablFree(IntPtr ptr);

        [DllImport("libbabl-0.1-0.dll", EntryPoint = "babl_polynomial_approximate_gamma")]
        internal extern static void BablPolynomialApproximateGamma(IntPtr poly, double gamma, double x0, double x1, int degree, int scale);
    }
}
