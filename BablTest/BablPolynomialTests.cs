﻿using babl;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using static BablTest.BablHandler;

namespace BablTest
{
    public unsafe class BablPolynomialTests
    {
        IntPtr poly;

        [SetUp]
        public void Init()
        {
            poly = BablAllocate(sizeof(BablPolynomialRaw.Big));
            Unsafe.InitBlock(poly.ToPointer(), 0, (uint)sizeof(BablPolynomialRaw.Big));
        }

        [TearDown]
        public void Exit() =>
            BablFree(poly);

        [TestCase(2.2)]
        [TestCase(1.0)]
        [TestCase(1.8)]
        [Category(Parity), Order(2)]
        public void ApproximateGammaTest(double gamma)
        {
            var x0 = 0.5 / 255.0;
            var x1 = 254.5 / 255.0;
            var degree = 6;
            var scale = 2;

            BablPolynomialApproximateGamma(poly, gamma, x0, x1, degree, scale);

            var actual = new BablPolynomial();

            BablPolynomial.ApproximateGamma(&actual, gamma, x0, x1, degree, scale);

            var expected = Marshal.PtrToStructure<BablPolynomialRaw>(poly);

            Assert.AreEqual(expected.degree, actual.degree);
            Assert.AreEqual(expected.scale, actual.scale);
            for (var i = 0; i < degree; i++)
                Assert.AreEqual(expected.coeff[i], actual.coeff[i]);

            TestContext.WriteLine("Expected: (ignore first 8 bytes)");
            TestContext.WriteLine(HexPrint(ref BablPolynomialRaw.AsRef(poly), 40));
            TestContext.WriteLine("Actual:");
            TestContext.WriteLine(HexPrint(ref actual, 40));
        }
    }
    public unsafe struct BablPolynomialRaw
    {
        public IntPtr eval;
        public int degree;
        public int scale;
        public fixed double coeff[11];

        public static ref BablPolynomialRaw AsRef(IntPtr ptr) =>
            ref Unsafe.AsRef<BablPolynomialRaw>((void*)ptr);

        public struct Big
        {
            public IntPtr eval;
            public int degree;
            public int scale;
            public fixed double coeff[23];
        }
    }
}
