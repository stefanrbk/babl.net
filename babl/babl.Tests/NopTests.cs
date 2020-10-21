using NUnit.Framework;

using System;

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

        [Test]
        public void DisplayAllTypes()
        {
            Babl.Init();

            Babl.TypeForEach(Write);
            Babl.ComponentForEach(Write);
            Babl.TrcForEach(Write);

            Babl.Exit();
        }

        private void Write(Babl b) =>
            Console.WriteLine($"{b}\n");
    }
}