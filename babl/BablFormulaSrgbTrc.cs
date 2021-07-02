using System.Linq;

namespace babl
{
    class BablFormulaSrgbTrc : BablFormulaTrc
    {
        internal BablFormulaSrgbTrc(double g, double a, double b, double c, double d, double e, double f)
            : base($"{g:6} {a:6} {b:4} {c: 4} {d:4} {e:4} {f:4}", g, new float[] { (float)g, (float)a, (float)b, (float)c, (float)d, (float)e, (float)f })
        {
            NonGammaCtor();
        }
        public override float FromLinear(float val)
        {
            float x = val;
            float a = lut[1];
            float b = lut[2];
            float c = lut[3];
            float d = lut[4];
            float e = lut[5];
            float f = lut[6];

            if (x - f > c * d)
            {
                float v = GammaFromLinear(x - f);
                v = (v - b) / a;
                return v is < 0 or >= 0
                    ? v : 0;
            }
            return c > 0
                ? (x - e) / c 
                : 0;
        }

        public override float ToLinear(float val)
        {
            float x = val;
            float a = lut[1];
            float b = lut[2];
            float c = lut[3];
            float d = lut[4];
            float e = lut[5];
            float f = lut[6];

            return x >= d
                ? GammaToLinear((a * x) + b) + e
                : (c * x) + f;
        }
        public static BablTrc New(double g, double a, double b, double c, double d, double e, double f)
        {
            int i;
            for (i = 0; trcDb[i] != null; i++)
                if (trcDb[i] is BablFormulaSrgbTrc trc &&
                    trc.gamma == g &&
                    trc.lut.Length == 7 &&
                    trc.lut.SequenceEqual(new float[] { (float)g, (float)a, (float)b, (float)c, (float)d, (float)e, (float)f }))
                    return trcDb[i]!;
            trcDb[i] = new BablFormulaSrgbTrc(g, a, b, c, d, e, f);
            return trcDb[i]!;
        }
    }
}
