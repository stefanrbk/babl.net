using babl;

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
    public unsafe class BablComponentTests
    {
        [Test, Category(Info), Order(1)]
        public void RegisteredComponents()
        {
            TestContext.WriteLine("Initially registered BablComponents:");
            babl.BablComponent.ForEach(b => TestContext.WriteLine(b.Name));
        }

        [Test, Category(Parity)]
        public void NewCbTest()
        {
            var expected = Marshal.PtrToStructure<BablComponentRaw>(BablComponentNew(__arglist("Cb", "id", 10032, "chroma", IntPtr.Zero)));

            var actual = (BablComponent)Babl.ComponentNew(name: "Cb", id: BablId.Cb, chroma: true);

            CheckSame(expected, actual);
        }

        [Test, Category(Parity)]
        public void NewYTest()
        {
            var expected = Marshal.PtrToStructure<BablComponentRaw>(BablComponentNew(__arglist("Y", "id", 10001, "luma", IntPtr.Zero)));
            var actual = (BablComponent)Babl.ComponentNew(name: "Y", id: BablId.GrayLinear, luma: true);

            CheckSame(expected, actual);
        }

        [Test]
        public void NewReturnsExistingComponent()
        {
            var expected = Babl.ComponentNew(name: "B'", id: BablId.BlueNonlinear, luma: true, chroma: true);
            var actual = Babl.ComponentNew(BablId.BlueNonlinear);

            CheckSame(expected as BablComponent, actual as BablComponent);
        }

        [Test]
        public void RetrieveAlphaTest()
        {
            var expected = Babl.ComponentNew(name: "A") as BablComponent;
            var actual = Babl.Component("A") as BablComponent;

            CheckSame(expected, actual);
        }

        static void CheckSame(BablComponentRaw expected, BablComponent actual)
        {
            Assert.AreEqual(Marshal.PtrToStringAnsi(expected.Instance.Name), actual.Name);
            Assert.AreEqual(expected.Instance.Id, actual.Id);
            Assert.AreEqual(expected.Luma, actual.Luma);
            Assert.AreEqual(expected.Chroma, actual.Chroma);
            Assert.AreEqual(expected.Alpha, actual.Alpha);
        }
        static void CheckSame(BablComponent expected, BablComponent actual)
        {
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Luma, actual.Luma);
            Assert.AreEqual(expected.Chroma, actual.Chroma);
            Assert.AreEqual(expected.Alpha, actual.Alpha);

            Assert.AreSame(expected, actual);
        }

        public unsafe struct BablComponentRaw
        {
            public BablHandler.Instance Instance;
            public bool Luma;
            public bool Chroma;
            public bool Alpha;
        }
    }
}
