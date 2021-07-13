using NUnit.Framework;

using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace BablTest
{
    internal static class Util
    {
        public unsafe static string HexPrint<T>(ref T ptr, int lineWidth) where T : unmanaged
        {
            var size = sizeof(T);

            var bytes = new ReadOnlySpan<byte>(Unsafe.AsPointer(ref ptr), size);
            var sb = new StringBuilder();

            var sizeChars = size.ToString("X").Length;
            var bytesPerLine = (lineWidth - (sizeChars + 5)) / 4;
            if (bytesPerLine < 1)
                throw new ArgumentException("Width is too small", nameof(lineWidth));

            for (var i = 0; i < size; i += bytesPerLine)
            {
                var strBytes = new StringBuilder(lineWidth + 1)
                    .Append(i.ToString("X")
                             .PadLeft(sizeChars, '0'))
                    .Append("   ");

                var strChars = new StringBuilder(bytesPerLine);

                for (var j = 0; j < bytesPerLine && j + i < size; j++)
                {
                    strBytes.Append(bytes[i + j].ToString("X")
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

        public static void CheckSame<T, U>(T expected, U actual) where T : struct
        {
            var tType = typeof(T);
            var uType = typeof(U);
            var instance = tType.GetField("Instance");

            var expectedFields = typeof(T).GetFields()
                .Where(f => f.Name != "Instance" && !f.Name.StartsWith('_'))
                .Concat(typeof(BablHandler.Instance).GetFields().Where(f => !f.Name.StartsWith('_')))
                .ToList();

            foreach (var field in expectedFields)
            {
                var f = uType.GetField(field.Name);
                var p = uType.GetProperty(field.Name);
                var member = f as MemberInfo ?? p;
                var type = f?.FieldType ?? p?.PropertyType;

                if (type is null)
                    Assert.Fail("{0} does not exist in {1}",
                                field.Name, uType.Name);

                object? expectedValue = null;
                if (field.DeclaringType == typeof(BablHandler.Instance))
                    if (type == typeof(string) && field.FieldType == typeof(IntPtr))
                        expectedValue = Marshal.PtrToStringAnsi((IntPtr)field.GetValue(instance.GetValue(expected)));
                    else
                        expectedValue = field.GetValue(instance.GetValue(expected));
                else if (type == typeof(string) && field.FieldType == typeof(IntPtr))
                    expectedValue = Marshal.PtrToStringAnsi((IntPtr)field.GetValue(expected));
                else
                    expectedValue = field.GetValue(expected);

                if (expectedValue is IntPtr ptr && ptr == IntPtr.Zero)
                    expectedValue = null;

                var actualValue = f?.GetValue(actual) ?? p?.GetValue(actual);
                if (actualValue is string s && s == string.Empty)
                    actualValue = null;


                Assert.AreEqual(expectedValue, actualValue, field.Name);
            }
        }
        public static void CheckSame<T>(T expected, T actual) => 
            Assert.AreSame(expected, actual);
    }
}
