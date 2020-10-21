using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace babl
{
    internal class BablTrc : Babl
    {
        internal const int MaxTrcs = 100;
        internal const double GammaX0 = 0.5 / 255.0;
        internal const double GammaX1 = 254.5 / 255.0;
        internal const int GammaDegree = 6;
        internal const int GammaScale = 2;

        private readonly static BablTrc?[] trcDb = new BablTrc?[MaxTrcs];
        private static BablTrc? trc;

        internal override BablClassType ClassType => BablClassType.Trc;

        public BablTrcType TrcType { get; set; }
        public int LutSize => Lut.Length;
        public double Gamma { get; set; }
        public float Rgamma { get; set; }
        public Func<Babl, float, float> FuncToLinear { get; set; } = null!;
        public Func<Babl, float, float> FuncFromLinear { get; set; } = null!;
        public Action<Babl, ReadOnlyMemory<float>, Memory<float>, int, int, int, int> FuncToLinearBuffered { get; set; } = null!;
        public Action<Babl, ReadOnlyMemory<float>, Memory<float>, int, int, int, int> FuncFromLinearBuffered { get; set; } = null!;
        public Polynomial GammaToLinearPoly { get; set; } = null!;
        public float GammaToLinearX0 { get; set; }
        public float GammaToLinearX1 { get; set; }
        public Polynomial GammaFromLinearPoly { get; set; } = null!;
        public float GammaFromLinearX0 { get; set; }
        public float GammaFromLinearX1 { get; set; }
        public float[] Lut { get; set; } = null!;
        public float[] InvLut { get; set; } = null!;

        private BablTrc(BablTrcType type, double gamma)
        {
            Id = 0;
            TrcType = type;
            Gamma = gamma > 0.0 ? gamma : 0.0;
            Rgamma = (float)(gamma > 0.0001 ? 1.0 / gamma : 0.0);
        }

        public static void FromLinearBuffered(Babl trc, ReadOnlyMemory<float> @in, Memory<float> @out, int inGap, int outGap, int components, int count) =>
            ((BablTrc)trc).FuncFromLinearBuffered(trc, @in, @out, inGap, outGap, components, count);

        public static void ToLinearBuffered(Babl trc, ReadOnlyMemory<float> @in, Memory<float> @out, int inGap, int outGap, int components, int count) =>
            ((BablTrc)trc).FuncToLinearBuffered(trc, @in, @out, inGap, outGap, components, count);

        public float FromLinear(float value) =>
            FuncFromLinear(this, value);

        public float ToLinear(float value) =>
            FuncToLinear(this, value);

        private static float Linear(Babl _, float value) =>
            value;

        public static float LutFromLinear(Babl trc, float x)
        {
            var Trc = (BablTrc)trc;
            var entry = (int)(x * (Trc.LutSize - 1));
            var diff = ((x * (Trc.LutSize - 1)) - entry);

            if (entry >= Trc.LutSize - 1)
            {
                entry = Trc.LutSize - 1;
                diff = 0.0f;
            }
            else if (entry < 0)
                entry = 0;

            if (diff > 0)
                return Trc.InvLut[entry] * (1.0f - diff) + Trc.InvLut[entry + 1] * diff;
            else
                return Trc.InvLut[entry];
        }

        public static float LutToLinear(Babl trc, float x)
        {
            var Trc = (BablTrc)trc;
            var entry = (int)(x * Trc.LutSize - 1);
            var diff = ((x * (Trc.LutSize - 1)) - entry);

            if (entry >= Trc.LutSize)
                entry = Trc.LutSize - 1;
            else if (entry < 0)
                entry = 0;

            if (diff > 0 && entry < Trc.LutSize - 1)
                return Trc.Lut[entry] * (1.0f - diff) + Trc.Lut[entry + 1] * diff;
            else
                return Trc.Lut[entry];
        }

        private static float GammaToLinear(Babl trc, float value)
        {
            var Trc = (BablTrc)trc;
            if (value >= Trc.GammaToLinearX0 &&
                value <= Trc.GammaToLinearX1)
                return (float)Trc.GammaToLinearPoly.Eval(value);
            else if (value > 0f)
                return MathF.Pow(value, (float)Trc.Gamma);
            return 0f;
        }

        private static float GammaFromLinear(Babl trc, float value)
        {
            var Trc = (BablTrc)trc;
            if (value >= Trc.GammaFromLinearX0 &&
                value <= Trc.GammaFromLinearX1)
                return (float)Trc.GammaFromLinearPoly.Eval(value);
            else if (value > 0f)
                return MathF.Pow(value, (float)Trc.Rgamma);
            return 0f;
        }

        private static void GammaToLinearBuffered(Babl trc, ReadOnlyMemory<float> @in, Memory<float> @out, int inGap, int outGap, int components, int count)
        {
            var inSpan = @in.Span;
            var outSpan = @out.Span;
            for (var i = 0; i < count; i++)
                for (var c = 0; c < components; c++)
                    @outSpan[outGap * i + c] = GammaToLinear(trc, @inSpan[inGap * i + c]);
        }

        private static void GammaFromLinearBuffered(Babl trc, ReadOnlyMemory<float> @in, Memory<float> @out, int inGap, int outGap, int components, int count)
        {
            var inSpan = @in.Span;
            var outSpan = @out.Span;
            for (var i = 0; i < count; i++)
                for (var c = 0; c < components; c++)
                    @outSpan[outGap * i + c] = GammaFromLinear(trc, @inSpan[inGap * i + c]);
        }

        private static float FormulaSrgbFromLinear(Babl trc, float value)
        {
            var Trc = (BablTrc)trc;
            (var x, var a, var b, var c, var d, var e, var f) = (value, Trc.Lut[1], Trc.Lut[2], Trc.Lut[3], Trc.Lut[4], Trc.Lut[5], Trc.Lut[6]);

            if (x - f > c * d)
            {
                var v = GammaFromLinear(trc, x - f);
                v = (v - b) / a;
                if (v is < 0 or >= 0)
                    return v;
                return 0;
            }
            if (c > 0)
                return (x - e) / c;
            return 0;
        }

        private static float FormulaSrgbToLinear(Babl trc, float value)
        {
            var Trc = (BablTrc)trc;
            (var x, var a, var b, var c, var d, var e, var f) = (value, Trc.Lut[1], Trc.Lut[2], Trc.Lut[3], Trc.Lut[4], Trc.Lut[5], Trc.Lut[6]);

            if (x >= d)
                return GammaToLinear(trc, a * x + b) + e;
            return c * x + f;
        }

        private static float FormulaCieFromLinear(Babl trc, float value)
        {
            var Trc = (BablTrc)trc;
            (var x, var a, var b, var c) = (value, Trc.Lut[1], Trc.Lut[2], Trc.Lut[3]);

            if (x>c)
            {
                var v = GammaFromLinear(trc, x - c);
                v = (v - b) / a;
                if (v is < 0 or >= 0)
                    return v;
            }
            return 0;
        }

        private static float FormulaCieToLinear(Babl trc, float value)
        {
            var Trc = (BablTrc)trc;
            (var x, var a, var b, var c) = (value, Trc.Lut[1], Trc.Lut[2], Trc.Lut[3]);

            if (x >= -b / a)
                return GammaToLinear(trc, a * x + b) + c;
            return c;
        }

        private static float SrgbToLinear(Babl _, float value) =>
            Utils.Gamma22ToLinear(value);

        private static float SrgbFromLinear(Babl _, float value) =>
            Utils.LinearToGamma22(value);

        private static void SrgbToLinearBuffered(Babl _, ReadOnlyMemory<float> @in, Memory<float> @out, int inGap, int outGap, int components, int count)
        {
            var inSpan = @in.Span;
            var outSpan = @out.Span;
            for (var i = 0; i < count; i++)
                for (var c = 0; c < components; c++)
                    @outSpan[outGap * i + c] = Utils.LinearToGamma22(@inSpan[inGap * i + c]);
        }

        private static void SrgbFromLinearBuffered(Babl _, ReadOnlyMemory<float> @in, Memory<float> @out, int inGap, int outGap, int components, int count)
        {
            var inSpan = @in.Span;
            var outSpan = @out.Span;
            for (var i = 0; i < count; i++)
                for (var c = 0; c < components; c++)
                    @outSpan[outGap * i + c] = Utils.LinearToGamma22(@inSpan[inGap * i + c]);
        }

        private static void ToLinearBufferedGeneric(Babl trc, ReadOnlyMemory<float> @in, Memory<float> @out, int inGap, int outGap, int components, int count)
        {
            var inSpan = @in.Span;
            var outSpan = @out.Span;
            for (var i = 0; i < count; i++)
                for (var c = 0; c < components; c++)
                    @outSpan[outGap * i + c] = ((BablTrc)trc).FuncToLinear(trc, @inSpan[inGap * i + c]);
        }

        private static void FromLinearBufferedGeneric(Babl trc, ReadOnlyMemory<float> @in, Memory<float> @out, int inGap, int outGap, int components, int count)
        {
            var inSpan = @in.Span;
            var outSpan = @out.Span;
            for (var i = 0; i < count; i++)
                for (var c = 0; c < components; c++)
                    @outSpan[outGap * i + c] = ((BablTrc)trc).FuncFromLinear(trc, @inSpan[inGap * i + c]);
        }

        private static void LinearBuffered(Babl _, ReadOnlyMemory<float> @in, Memory<float> @out, int inGap, int outGap, int components, int count)
        {
            var inSpan = @in.Span;
            var outSpan = @out.Span;
            for (var i = 0; i < count; i++)
                for (var c = 0; c < components; c++)
                    @outSpan[outGap * i + c] = @inSpan[inGap * i + c];
        }

        public static Babl? Find(string name)
        {
            for (var i = 0; trcDb[i] is not null; i++)
                if (trcDb[i]!.Name == name)
                    return trcDb[i];
            Log($"failed to find trc '{name}'\n");
            return null;
        }

        public static Babl? Create(string name,
                                  BablTrcType type,
                                  double gamma,
                                  int lutNum,
                                  float[] lut)
        {
            trc = new BablTrc(type, gamma);

            int i;
            if (lutNum is not 0)
            {
                for (i = 0; trcDb[i] is not null; i++)
                    if (trcDb[i]!.LutSize == lutNum && trcDb[i]!.Lut.SequenceEqual(lut))
                        return trcDb[i]!;
            }
            else
                for (i = 0; trcDb[i] is not null; i++)
                    if (trcDb[i]!.Equals(type, lutNum, gamma))
                        return trcDb[i]!;
            if (i >= MaxTrcs - 1)
            {
                Log("too many BablTrcs");
                return null;
            }

            if (name is not "")
                trc.Name = name;
            else if (lutNum is not 0)
                trc.Name = "lut-trc";
            else
                trc.Name = $"trc-{type}-{gamma}";

            if (lutNum is not 0)
            {
                int j;
                trc.Lut = new float[lutNum];
                trc.InvLut = new float[lutNum];
                lut.CopyTo(trc.Lut, 0);

                for (j = 0; j < lutNum; j++)
                {
                    int k;
                    var min = 0.0;
                    var max = 1.0;
                    for (k = 0; k < 16; k++)
                    {
                        var guess = (min + max) / 2;
                        var reversedIndex = LutToLinear(trc, (float)guess) * (lutNum - 1.0);

                        if (reversedIndex < j)
                            min = guess;
                        else if (reversedIndex > j)
                            max = guess;
                    }
                    trc.InvLut[j] = (float)(min + max) / 2;
                }
            }

            trc.FuncToLinearBuffered = ToLinearBufferedGeneric;
            trc.FuncFromLinearBuffered = FromLinearBufferedGeneric;

            switch (trc.TrcType)
            {
                case BablTrcType.Linear:
                    trc.FuncToLinear = Linear;
                    trc.FuncFromLinear = Linear;
                    trc.FuncFromLinearBuffered = LinearBuffered;
                    trc.FuncToLinearBuffered = LinearBuffered;
                    break;
                case BablTrcType.FormulaGamma:
                    trc.FuncToLinear = GammaToLinear;
                    trc.FuncFromLinear = GammaFromLinear;
                    trc.FuncToLinearBuffered = GammaToLinearBuffered;
                    trc.FuncFromLinearBuffered = GammaFromLinearBuffered;

                    trc.GammaToLinearX0 = (float)GammaX0;
                    trc.GammaToLinearX1 = (float)GammaX1;
                    trc.GammaToLinearPoly = Polynomial.ApproximateGamma(trc.Gamma, trc.GammaToLinearX0, trc.GammaToLinearX1, GammaDegree, GammaScale);

                    trc.GammaFromLinearX0 = (float)GammaX0;
                    trc.GammaFromLinearX1 = (float)GammaX1;
                    trc.GammaFromLinearPoly = Polynomial.ApproximateGamma(trc.Rgamma, trc.GammaFromLinearX0, trc.GammaFromLinearX1, GammaDegree, GammaScale);

                    break;
                case BablTrcType.FormulaCie:
                    trc.Lut = new float[4];
                    {
                        for (var j = 0; j < 4; j++)
                            trc.Lut[j] = lut[j];
                    }
                    trc.FuncToLinear = FormulaCieToLinear;
                    trc.FuncFromLinear = FormulaCieFromLinear;

                    trc.GammaToLinearX0 = lut[4];
                    trc.GammaToLinearX1 = (float)GammaX1;
                    trc.GammaToLinearPoly = Polynomial.ApproximateGamma(trc.Rgamma, trc.GammaToLinearX0, trc.GammaToLinearX1, GammaDegree, GammaScale);

                    trc.GammaFromLinearX0 = lut[3] * lut[4];
                    trc.GammaFromLinearX1 = (float)GammaX1;
                    trc.GammaFromLinearPoly = Polynomial.ApproximateGamma(trc.Rgamma, trc.GammaFromLinearX0, trc.GammaFromLinearX1, GammaDegree, GammaScale);

                    break;
                case BablTrcType.FormulaSrgb:
                    trc.Lut = new float[7];
                    {
                        for (var j = 0; j < 7; j++)
                            trc.Lut[j] = lut[j];
                    }
                    trc.FuncToLinear = FormulaSrgbToLinear;
                    trc.FuncFromLinear = FormulaSrgbFromLinear;

                    trc.GammaToLinearX0 = lut[4];
                    trc.GammaToLinearX1 = (float)GammaX1;
                    trc.GammaToLinearPoly = Polynomial.ApproximateGamma(trc.Gamma, trc.GammaToLinearX0, trc.GammaToLinearX1, GammaDegree, GammaScale);

                    trc.GammaFromLinearX0 = lut[3] * lut[4];
                    trc.GammaFromLinearX1 = (float)GammaX1;
                    trc.GammaFromLinearPoly = Polynomial.ApproximateGamma(trc.Rgamma, trc.GammaFromLinearX0, trc.GammaFromLinearX1, GammaDegree, GammaScale);

                    break;
                case BablTrcType.Srgb:
                    trc.FuncToLinear = SrgbToLinear;
                    trc.FuncFromLinear = SrgbFromLinear;
                    trc.FuncFromLinearBuffered = SrgbFromLinearBuffered;
                    trc.FuncToLinearBuffered = SrgbToLinearBuffered;
                    break;
                case BablTrcType.Lut:
                    trc.FuncToLinear = LutToLinear;
                    trc.FuncFromLinear = LutFromLinear;
                    break;
            }

            trcDb[i] = trc;
            return trc;
        }

        public bool Equals(BablTrcType type,
                           int lutSize,
                           double gamma) =>
            TrcType == type &&
            LutSize == lutSize &&
            Gamma == gamma;

        public override IEnumerator<Babl> GetEnumerator() =>
            (IEnumerator<Babl>)(from val in trcDb
                                where val is not null
                                select val);

        public static Babl? FormulaSrgb(double g, double a, double b, double c, double d, double e, double f)
        {
            var @params = new float[]{ (float)g, (float)a, (float)b, (float)c, (float)d, (float)e, (float)f};

            if (Math.Abs(g - 2.400) < 0.01 &&
                Math.Abs(a - 0.947) < 0.01 &&
                Math.Abs(b - 0.052) < 0.01 &&
                Math.Abs(c - 0.077) < 0.01 &&
                Math.Abs(d - 0.040) < 0.01 &&
                Math.Abs(e - 0.000) < 0.01 &&
                Math.Abs(f - 0.000) < 0.01
                )
                return Find("sRGB");
            var name = $"{g:F6} {a:F6} {b:F4} {c:F4} {d:F4} {e:F4} {f:F4}"
                           .Replace(',', '.');

            return Create(name, BablTrcType.FormulaSrgb, g, 0, @params);
        }

        public static Babl? FormulaCie(double g, double a, double b, double c)
        {
            var @params = new float[] { (float)g, (float)a, (float)b, (float)c };

            var name = $"{g:F6} {a:F6} {b:F4} {c:F4}"
                           .Replace(',', '.');

            return Create(name, BablTrcType.FormulaCie, g, 0, @params);
        }

        public static Babl? FormulaGamma(double gamma)
        {
            if (Math.Abs(gamma - 1.0) < 0.01)
                return Create("linear", BablTrcType.Linear, 1.0, 0, Array.Empty<float>());

            var name = $"{gamma:F6}"
                           .Replace(',', '.');

            return Create(name, BablTrcType.FormulaGamma, gamma, 0, Array.Empty<float>());
        }

        public static new void Init()
        {
            Create("sRGB", BablTrcType.Srgb, 2.2, 0, Array.Empty<float>());
            FormulaGamma(2.2);
            FormulaGamma(1.8);
            FormulaGamma(1.0);
            Create("linear", BablTrcType.Linear, 1.0, 0, Array.Empty<float>());
        }

        public static bool LutMatchGamma(float[] lut,
                                        int lutSize,
                                        float gamma)
        {
            var match = true;
            var tolerence = lutSize > 1024 ? 0.0001f : 0.001f;

            for (var i = 0; match && i < lutSize; i++)
                if (MathF.Abs(lut[i] - MathF.Pow((i / (lutSize - 1.0f)), gamma)) > tolerence)
                    match = false;
            return match;
        }

        public static Babl? LutFind(float[] lut,
                             int lutSize)
        {
            var match = true;

            // look for linear match
            for (var i = 0; match && i < lutSize; i++)
                if (MathF.Abs(lut[i] - i / (lutSize - 1.0f)) > 0.015f)
                    match = false;
            if (match)
                return FormulaGamma(1.0);

            // look for sRGB match
            match = true;
            var tolerence = lutSize > 1024 ? 0.0001f : 0.001f;

            for (var i = 0; match && i < lutSize; i++)
                if (MathF.Abs(lut[i] - Utils.Gamma22ToLinear(i / (lutSize - 1.0f))) > tolerence)
                    match = false;

            if (match)
                return Find("sRGB");

            if (LutMatchGamma(lut, lutSize, 2.2f))
                return FormulaGamma(2.2);

            if (LutMatchGamma(lut, lutSize, 1.8f))
                return FormulaGamma(1.8);

            return null;
        }
    }
}
