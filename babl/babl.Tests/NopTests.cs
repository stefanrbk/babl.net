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

            Console.WriteLine("Types");
            Babl.TypeForEach(Write);
            Console.WriteLine("Components");
            Babl.ComponentForEach(Write);
            Console.WriteLine("TRCs");
            Babl.TrcForEach(Write);
            Console.WriteLine("Conversions");
            Babl.ConversionForEach(Write);

            Babl.Exit();
        }

        private void Write(Babl b) =>
            Console.WriteLine($"{b}\n");
    }
}