using babl;
using NUnit.Framework.Internal;
using NUnit.Framework;

using static BablTest.BablHandler;
using static BablTest.Util;
using System.Runtime.InteropServices;
using System;
using System.Runtime.CompilerServices;

namespace BablTest
{
    [TestFixture(TestOf = typeof(BablType))]
    public unsafe class BablTypeTests
    {
        [Test, BaseInfo]
        public void RegisteredTypes()
        {
            TestContext.WriteLine("Initially registered BablTypes:");
            babl.BablType.ForEach(b => TestContext.WriteLine(b.Name));
        }

        [Test, BaseParity]
        public void NewFloatTest()
        {
            var expected = Marshal.PtrToStructure<Raw>(BablTypeNew(__arglist("float", "id", BablId.Float, "bits", 32, "doc", "IEEE 754 single precision", 0)));
            var actual = (BablType)Babl.TypeNew(name: "float", id: BablId.Float, bits: 32, docs: "IEEE 754 single precision");

            CheckSame(expected, actual);
        }

        [Test, BaseParity]
        public void NewReturnsExistingComponent()
        {

        }

        unsafe struct Raw
        {
            public Instance Instance;
            public IntPtr _FromList;
            public int Bits;
            public double _MinVal;
            public double _MaxVal;

            public static ref Raw AsRef(IntPtr ptr) =>
                ref Unsafe.AsRef<Raw>((void*)ptr);
        }
    }
}
