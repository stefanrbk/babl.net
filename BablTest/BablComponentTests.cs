using babl;

using NUnit.Framework;
using NUnit.Framework.Internal;

using System;
using System.Runtime.InteropServices;

using static BablTest.BablHandler;
using static BablTest.Util;

namespace BablTest
{
    [TestFixture(TestOf = typeof(BablComponent))]
    public unsafe class BablComponentTests
    {
        [BaseInfo, Test]
        public void RegisteredComponents()
        {
            TestContext.WriteLine("Initially registered BablComponents:");
            babl.BablComponent.ForEach(b => TestContext.WriteLine(b.Name));
        }

        [BaseParity, Test]
        public void NewCbTest()
        {
            var expected = Marshal.PtrToStructure<Raw>(BablComponentNew(__arglist("Cb", "id", 10032, "chroma", IntPtr.Zero)));

            var actual = (BablComponent)Babl.ComponentNew(name: "Cb", id: BablId.Cb, chroma: true);

            CheckSame(expected, actual);
        }

        [BaseParity, Test]
        public void NewYTest()
        {
            var expected = Marshal.PtrToStructure<Raw>(BablComponentNew(__arglist("Y", "id", 10001, "luma", IntPtr.Zero)));
            var actual = (BablComponent)Babl.ComponentNew(name: "Y", id: BablId.GrayLinear, luma: true);

            CheckSame(expected, actual);
        }

        [BaseIdentity, Test]
        public void NewReturnsExistingComponent()
        {
            var expected = Babl.ComponentNew(name: "B'", id: BablId.BlueNonlinear, luma: true, chroma: true);
            var actual = Babl.ComponentNew(BablId.BlueNonlinear);

            CheckSame(expected as BablComponent, actual as BablComponent);
        }

        [BaseIdentity, Test]
        public void RetrieveAlphaTest()
        {
            var expected = Babl.ComponentNew(name: "A") as BablComponent;
            var actual = Babl.Component("A") as BablComponent;

            CheckSame(expected, actual);
        }

        unsafe struct Raw
        {
            public Instance Instance;
            public bool Luma;
            public bool Chroma;
            public bool Alpha;
        }
    }
}
