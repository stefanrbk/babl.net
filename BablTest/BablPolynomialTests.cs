using babl;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BablTest
{
    public unsafe class BablPolynomialTests
    {
        IntPtr poly;

        [SetUp]
        public void Init()
        {
            poly = BablHandler.BablAllocate(sizeof(BablPolynomialRaw.Big));
            Unsafe.InitBlock(poly.ToPointer(), 0, (uint)sizeof(BablPolynomialRaw.Big));
        }

        [TearDown]
        public void Exit() =>
            BablHandler.BablFree(poly);

        [TestCase(2.2)]
        [TestCase(1.0)]
        [TestCase(1.8)]
        [Category("Parity")]
        public void ApproximateGammaTest(double gamma)
        {
            var x0 = 0.5 / 255.0;
            var x1 = 254.5 / 255.0;
            var degree = 6;
            var scale = 2;

            BablHandler.BablPolynomialApproximateGamma(poly, gamma, x0, x1, degree, scale);

            var actual = new BablPolynomial();

            BablPolynomial.ApproximateGamma(&actual, gamma, x0, x1, degree, scale);

            var expected = Marshal.PtrToStructure<BablPolynomialRaw>(poly);

            Assert.AreEqual(expected.degree, actual.degree);
            Assert.AreEqual(expected.scale, actual.scale);
            for (var i = 0; i < degree; i++)
                Assert.AreEqual(expected.coeff[i], actual.coeff[i]);

            TestContext.WriteLine("Expected: (ignore first 8 bytes)");
            TestContext.WriteLine(BablHandler.HexPrint(poly, sizeof(BablPolynomialRaw), 40));
            TestContext.WriteLine("Actual:");
            TestContext.WriteLine(BablHandler.HexPrint((IntPtr)Unsafe.AsPointer(ref actual), sizeof(BablPolynomial), 40));
        }
    }
    public unsafe struct BablPolynomialRaw
    {
        public IntPtr eval;
        public int degree;
        public int scale;
        public fixed double coeff[11];

        public struct Big
        {
            public IntPtr eval;
            public int degree;
            public int scale;
            public fixed double coeff[23];
        }
    }
}
