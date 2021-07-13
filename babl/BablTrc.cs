using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace babl
{
#if DEBUG
    public
#else
    internal
#endif
    abstract class BablTrc : Babl
    {
        protected const int MaxTrcs = 100;
        protected const float GammaX0 = 0.5f / 255;
        protected const float GammaX1 = 254.5f / 255;
        protected const int GammaDegree = 6;
        protected const int GammaScale = 2;

        protected static readonly BablTrc?[] trcDb = new BablTrc[MaxTrcs];

        protected double gamma;
        protected float rgamma;
        protected BablPolynomial gammaToLinear;
        protected float gammaToLinearX0;
        protected float gammaToLinearX1;
        protected BablPolynomial gammaFromLinear;
        protected float gammaFromLinearX0;
        protected float gammaFromLinearX1;

        public static Babl? Find(string name)
        {
            for (var i = 0; trcDb[i] is not null; i++)
                if (string.Equals(trcDb[i]!.Name, name))
                    return trcDb[i];
            Log($"failed to find trc {name}");
            return null;
        }

        protected BablTrc(string name, double gamma)
            : base(name, 0, "")
        {
            this.gamma = gamma > 0.0 ? gamma : 0.0;
            rgamma = gamma > 1e-4 ? (float)(1 / gamma) : 0;
        }

        public abstract float ToLinear(float val);
        public abstract float FromLinear(float val);
        public virtual void ToLinear(in ReadOnlySpan<float> @in,
                                     Span<float> @out,
                                     int inGap,
                                     int outGap,
                                     int components,
                                     int count)
        {
            for (var i = 0; i < count; i++)
                for (var c = 0; c < components; c++)
                    @out[(outGap * i) + c] = ToLinear(@in[(inGap * i) + c]);
        }
        public virtual void FromLinear(in ReadOnlySpan<float> @in,
                                       Span<float> @out,
                                       int inGap,
                                       int outGap,
                                       int components,
                                       int count)
        {
            for (var i = 0; i < count; i++)
                for (var c = 0; c < components; c++)
                    @out[(outGap * i) + c] = FromLinear(@in[(inGap * i) + c]);
        }

        public unsafe float GammaFromLinear(float value)
        {
            if (value >= gammaFromLinearX0 &&
                value <= gammaFromLinearX1)
                fixed (BablPolynomial* poly = &gammaFromLinear)
                    return (float)BablPolynomial.Eval(poly, value);
            else if (value > 0)
                return MathF.Pow(value, rgamma);
            return 0;
        }

        public unsafe float GammaToLinear(float value)
        {
            if (value >= gammaToLinearX0 &&
                value <= gammaToLinearX1)
                fixed (BablPolynomial* poly = &gammaToLinear)
                    return (float)BablPolynomial.Eval(poly, value);
            else if (value > 0)
                return MathF.Pow(value, (float)gamma);
            return 0;
        }
#if DEBUG
        public static void RemoveTrc(BablTrc trc)
        {
            var i = 0;
            for (; i < trcDb.Length; i++)
                if (trcDb[i] is not null)
                    if (trcDb[i]!.Equals(trc))
                    {
                        trcDb[i] = null;
                        break;
                    }
            for (; i < trcDb.Length - 1; i++)
                trcDb[i] = trcDb[i+1];
        }
#endif
    }
}
