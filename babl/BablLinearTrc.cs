using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace babl
{
    class BablLinearTrc : BablTrc
    {
        internal BablLinearTrc()
            : base("linear", 1.0) { }
        public override float FromLinear(float value) =>
            value;

        public override void FromLinear(in ReadOnlySpan<float> @in, Span<float> @out, int inGap, int outGap, int components, int count)
        {
            for (var i = 0; i < count; i++)
                for (var c = 0; c < components; c++)
                    @out[(i * outGap) + c] = @in[(i * inGap) + c];
        }

        public override float ToLinear(float value) => 
            value;

        public override void ToLinear(in ReadOnlySpan<float> @in, Span<float> @out, int inGap, int outGap, int components, int count) =>
            FromLinear(@in, @out, inGap, outGap, components, count);
    }
}
