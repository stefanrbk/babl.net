using babl;
using NUnit.Framework.Internal;
using NUnit.Framework;

using static BablTest.BablHandler;

namespace BablTest
{
    [TestFixture(TestOf = typeof(BablType))]
    public unsafe class BablTypeTests
    {
        [Test, Category(Info), Order(1)]
        public void RegisteredTypes()
        {
            TestContext.WriteLine("Initially registered BablTypes:");
            BablType.ForEach(b => TestContext.WriteLine(b.Name));
        }

    }
}
