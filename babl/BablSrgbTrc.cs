using System;

using static babl.Util;

namespace babl
{
    class BablSrgbTrc : BablTrc
    {
        internal BablSrgbTrc(double gamma)
            : base("sRGB", gamma) { }

        public override float FromLinear(float val) =>
            LinearToGamma2dot2(val);

        public override void FromLinear(in ReadOnlySpan<float> @in, Span<float> @out, int inGap, int outGap, int components, int count)
        {
            for (var i = 0; i < count; i++)
                for (var c = 0; c < components; c++)
                    @out[(outGap * i) + c] = LinearToGamma2dot2(@in[(inGap * i) + c]);
        }

        public override float ToLinear(float val) =>
            Gamma2dot2ToLinear(val);

        public override void ToLinear(in ReadOnlySpan<float> @in, Span<float> @out, int inGap, int outGap, int components, int count)
        {
            for (var i = 0; i < count; i++)
                for (var c = 0; c < components; c++)
                    @out[(outGap * i) + c] = Gamma2dot2ToLinear(@in[(inGap * i) + c]);
        }
    }
}
