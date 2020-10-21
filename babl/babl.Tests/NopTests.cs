using NUnit.Framework;

namespace babl.Tests
{
    public class NopTests
    {
        [Test]
        public void Nop()
        {
            Babl.Init();
            Babl.Exit();
        }
    }
}