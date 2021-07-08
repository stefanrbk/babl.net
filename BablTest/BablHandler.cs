using NUnit.Framework;

using System;
using System.Runtime.InteropServices;

namespace BablTest
{
    [SetUpFixture]
    public class BablHandler
    {
        const string DllPath = "libbabl-0.1-0.dll";

        public const string Info = "Info";
        public const string Parity = "Parity";
        public const string Identity = "Identity";

        [OneTimeSetUp]
        public void Init() => 
            BablInit();

        [OneTimeTearDown]
        public void Exit() =>
            BablExit();

        [DllImport(DllPath, EntryPoint = "babl_init")]
        internal extern static void BablInit();

        [DllImport(DllPath, EntryPoint = "babl_exit")]
        internal extern static void BablExit();

        [DllImport(DllPath, EntryPoint = "babl_malloc")]
        internal extern static IntPtr BablAllocate(nint size);

        [DllImport(DllPath, EntryPoint = "babl_free")]
        internal extern static void BablFree(IntPtr ptr);

        [DllImport(DllPath, EntryPoint = "babl_polynomial_approximate_gamma")]
        internal extern static void BablPolynomialApproximateGamma(IntPtr poly, double gamma, double x0, double x1, int degree, int scale);

        [DllImport(DllPath, EntryPoint = "babl_component", CharSet = CharSet.Ansi)]
        internal extern static IntPtr BablComponent(string name);

        [DllImport(DllPath, EntryPoint = "babl_component_new", CharSet = CharSet.Ansi)]
        internal extern static IntPtr BablComponentNew(__arglist);

        public unsafe struct Instance
        {
            public int ClassType;
            public int Id;
            public IntPtr Creator;
            public IntPtr Name;
            public IntPtr Docs;
        }
    }
}
