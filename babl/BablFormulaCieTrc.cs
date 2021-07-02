namespace babl
{
    class BablFormulaCieTrc : BablFormulaTrc
    {
        internal BablFormulaCieTrc(double g, double a, double b, double c)
            : base($"{g:6} {a:6} {b:4} {c: 4}", g, new float[] { (float)g, (float)a, (float)b, (float)c })
        {
            NonGammaCtor();
        }

        public override float FromLinear(float val)
        {
            var x = val;
            var a = lut[1];
            var b = lut[2];
            var c = lut[3];

            if (x > c)
            {
                var v = GammaFromLinear(x - c);
                v = (v - b) / a;
                if (v is < 0 or >= 0)
                    return v;
            }
            return 0;
        }

        public override float ToLinear(float val)
        {
            var x = val;
            var a = lut[1];
            var b = lut[2];
            var c = lut[3];

            return x >= -b / a
                ? GammaToLinear((a * x) + b)
                : c;
        }
    }
}
