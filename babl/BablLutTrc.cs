using System.Linq;

namespace babl
{
    class BablLutTrc : BablTrc
    {
        float[] lut;
        float[] invLut;

        internal BablLutTrc(string name, float[] entries)
            : base(name == "" ? "lut-trc" : name, 0)
        {
            lut = entries;
            invLut = new float[entries.Length];

            for (var j = 0; j < entries.Length; j++)
            {
                var min = 0.0;
                var max = 1.0;

                for (var k = 0; k < 16; k++)
                {
                    var guess = (min + max) / 2;
                    var reversedIndex = LutToLinear((float)guess) * (entries.Length - 1);

                    if (reversedIndex < j)
                        min = guess;
                    else if (reversedIndex > j)
                        max = guess;
                }
                invLut[j] = (float)(min + max) / 2;
            }
        }

        float LutFromLinear(float x)
        {
            var entry = (int)(x * (lut.Length - 1));
            var diff = (x * (lut.Length - 1)) - entry;

            if (entry >= lut.Length - 1)
                (entry, diff) = (lut.Length - 1, 0);
            else if (entry < 0)
                entry = 0;

            return diff > 0
               ? (invLut[entry] * (1 - diff)) + (invLut[entry + 1] * diff)
               : invLut[entry];
        }

        float LutToLinear(float x)
        {
            var entry = (int)(x * (lut.Length - 1));
            var diff = (x * (lut.Length - 1)) - entry;

            if (entry >= lut.Length -1)
                entry = lut.Length - 1;
            else if (entry < 0)
                entry = 0;

             return diff > 0 && entry < lut.Length - 1
                ? (lut[entry] * (1 - diff)) + (lut[entry + 1] * diff)
                : lut[entry];
        }

        public override float FromLinear(float val) =>
            LutFromLinear(val);

        public override float ToLinear(float val) =>
            LutToLinear(val);

        public static BablTrc New(string name, float[] lut)
        {
            for (var i = 0; trcDb[i] != null; i++)
                if (trcDb[i] is BablLutTrc trc &&
                    trc.lut.Length == lut.Length &&
                    trc.lut.SequenceEqual(lut))
                    return trcDb[i]!;
            return new BablLutTrc(name, lut);
        }
    }
}
