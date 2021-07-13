using babl;

using NUnit.Framework;

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using static BablTest.BablHandler;
using static BablTest.Util;

namespace BablTest
{
    [TestFixture(TestOf = typeof(BablPolynomial))]
    public unsafe class BablPolynomialTests
    {
        IntPtr poly;

        [SetUp]
        public void Init()
        {
            poly = BablAllocate(sizeof(Raw.Big));
            Unsafe.InitBlock(poly.ToPointer(), 0, (uint)sizeof(Raw.Big));
        }

        [TearDown]
        public void Exit() =>
            BablFree(poly);

        [TestCase(2.2)]
        [TestCase(1.0)]
        [TestCase(1.8)]
        [BaseParity]
        public void ApproximateGammaTest(double gamma)
        {
            var x0 = 0.5 / 255.0;
            var x1 = 254.5 / 255.0;
            var degree = 6;
            var scale = 2;

            BablPolynomialApproximateGamma(poly, gamma, x0, x1, degree, scale);

            var actual = new BablPolynomial();

            BablPolynomial.ApproximateGamma(&actual, gamma, x0, x1, degree, scale);

            var expected = Marshal.PtrToStructure<Raw>(poly);

            Assert.AreEqual(expected.degree, actual.degree);
            Assert.AreEqual(expected.scale, actual.scale);
            for (var i = 0; i < degree; i++)
                Assert.AreEqual(expected.coeff[i], actual.coeff[i]);

            TestContext.WriteLine("Expected: (ignore first 8 bytes)");
            TestContext.WriteLine(HexPrint(ref Raw.AsRef(poly), 40));
            TestContext.WriteLine("Actual:");
            TestContext.WriteLine(HexPrint(ref actual, 40));
        }
        unsafe struct Raw
        {
            public IntPtr eval;
            public int degree;
            public int scale;
            public fixed double coeff[11];

            public static ref Raw AsRef(IntPtr ptr) =>
                ref Unsafe.AsRef<Raw>((void*)ptr);

            public struct Big
            {
                public IntPtr eval;
                public int degree;
                public int scale;
                public fixed double coeff[23];
            }
        }
    }
}
