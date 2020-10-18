using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace babl
{
    internal record Polynomial
    {
        internal const int MaxDegree = 10;
        internal const int MinDegree = 0;
        internal const int MaxScale = 2;
        internal const int MinScale = 1;
        internal const int MaxBigDegree = 2 * MaxDegree + MaxScale;
        internal const double Epsilon = 1e-10;

        static Polynomial()
        {
            for (var i = 0; i < MaxDegree; i++)
            {
                evalFuncs[0, i] = (p, d) => EvalDegree1(p, d, i);
                evalFuncs[1, i] = (p, d) => EvalDegree2(p, d, i);
            }
        }
        private readonly static Func<Polynomial, double, double>[,] evalFuncs = new Func<Polynomial, double, double>[MaxScale, MaxBigDegree + 1];
        internal readonly static Polynomial Empty = new Polynomial(0, 1);
        private readonly int degree;
        private readonly int scale;

        public int Degree
        {
            get => degree;
            init
            {
                Babl.Assert(value is >= MinDegree and <= MaxBigDegree);
                degree = value;
            }
        }
        public int Scale
        {
            get => scale;
            init
            {
                Babl.Assert(value is >= MinScale and <= MaxScale);
                scale = value;
            }
        }
        internal double[] Coeff { get; init; }

        public Polynomial(int degree, int scale)
        {
            this.degree = 0;
            this.scale = 0;
            Coeff = new double[degree];

            Degree = degree;
            Scale = scale;
        }

        public double Eval(double x) =>
            evalFuncs[Scale - 1, Degree](this, x);

        private static double EvalDegree1(Polynomial poly, double x, int degree)
        {
            var x2 = x * x;
            return EvalCoeff(poly, x2, degree) + EvalCoeff(poly, x2, degree - 1) * x;
        }

        private static double EvalDegree2(Polynomial poly, double x, int degree) =>
            EvalCoeff(poly, x, degree) + EvalCoeff(poly, x, degree - 1) * Math.Sqrt(x);

        private static double EvalCoeff(Polynomial poly, double x, int degree) =>
            degree switch
            {
                >= 2 => EvalCoeff(poly, x, degree - 2) * x + poly.Coeff[degree],
                >= 0 => poly.Coeff[degree],
                _ => 0.0
            };

        public Polynomial Shrink()
        {
            int i;
            for (i = 0; i <= degree + 1; i++)
            {
                if (i == degree + 1)
                    break; // short circuit before next if breaks the array
                if (Math.Abs(Coeff[i]) > Epsilon)
                    break;
            }

            if (i == degree+1)
                return this with { Degree = 0 };
            else if (i > 0)
            {
                var nPoly = this with 
                { 
                    Degree = degree - i,
                    Coeff = Coeff.Skip(i).ToArray()
                };
                return nPoly;
            }
            return this;
        }

        public Polynomial Add(Polynomial poly)
        {
            Babl.Assert(scale == poly.scale);
                        
            return this with 
            { 
                Degree = (degree >= poly.degree) ? degree : poly.degree,
                Coeff = Coeff
                            .Zip(poly.Coeff, (f, s) => f + s)
                            .Concat((degree >= poly.degree ? this : poly).Coeff
                                .Skip((degree >= poly.degree ? poly : this).degree))
                            .ToArray()
            };
        }

        public Polynomial Sub(Polynomial poly)
        {
            Babl.Assert(scale == poly.scale);

            return this with
            {
                Degree = (degree >= poly.degree) ? degree : poly.degree,
                Coeff = Coeff
                            .Zip(poly.Coeff, (f, s) => f - s)
                            .Concat((degree >= poly.degree ? this : poly).Coeff
                                .Skip((degree >= poly.degree ? poly : this).degree))
                            .ToArray()
            };
        }

        public Polynomial Mul(double s) =>
            this with
            {
                Coeff = Coeff.Select(a => a * s).ToArray()
            };
        public Polynomial Div(double s) =>
            this with
            {
                Coeff = Coeff.Select(a => a / s).ToArray()
            };

        public Polynomial Mul(Polynomial poly)
        {
            Babl.Assert(scale == poly.scale);

            var coeff = new double[degree + poly.degree];

            for (var i = 0; i <= degree; i++)
                for (var j = 0; j <= poly.degree; j++)
                    coeff[i + j] += Coeff[i] * poly.Coeff[j];
            return new Polynomial(degree + poly.degree, scale) { Coeff = coeff };
        }

        public Polynomial Integrate()
        {
            var coeff = new double[degree + scale];
            Coeff.CopyTo(coeff, 0);
            var i = 0;
            for (; i<= degree - scale; i++)
            {
                coeff[i] *= scale;
                coeff[i] /= degree - i;
            }
            for (; i <= degree; i++)
                coeff[i] = 0.0;

            return this with
            {
                Degree = degree + scale,
                Coeff = coeff,
            };
        }

        public Polynomial GammaIntegrate(double gamma)
        {
            var coeff = new double[degree + scale];
            Coeff.CopyTo(coeff, 0);
            gamma *= scale;

            var i = 0;
            for (; i<= degree - scale; i++)
            {
                coeff[i] *= scale;
                coeff[i] /= degree - i + gamma;
            }
            for (; i <= degree; i++)
                coeff[i] = 0.0;

            return this with
            {
                Degree = degree + scale,
                Coeff = coeff,
            };
        }

        public double InnerProduct(Polynomial poly, double x0, double x1)
        {
            var temp = Mul(poly).Integrate();
            return temp.Eval(x1) - temp.Eval(x0);
        }

        public double GammaInnerProduct(double gamma, double x0, double x1)
        {
            var temp = GammaIntegrate(gamma);
            return temp.Eval(x1) * Math.Pow(x1, gamma) -
                   temp.Eval(x0) * Math.Pow(x0, gamma);
        }

        public double Norm(double x0, double x1) =>
            Math.Sqrt(InnerProduct(this, x0, x1));

        public Polynomial Normalize(double x0, double x1)
        {
            var norm = Norm(x0, x1);
            if (norm > Epsilon)
                return Div(norm);
            return this;
        }
        public static Polynomial[] Basis(int n, int scale)
        {
            var basis = new Polynomial[n];

            for (var i = 0; i < n; i++)
                basis[i] = new Polynomial(i, scale);

            return basis;
        }

        public static Polynomial[] OrthonormalBasis(int n, double x0, double x1, int scale) =>
            Basis(n, scale).GramSchmidt(n, x0, x1);

        public static Polynomial ApproximateGamma(double gamma, double x0, double x1, int degree, int scale)
        {
            Babl.Assert(gamma >= 0.0);
            Babl.Assert(x0 >= 0.0);
            Babl.Assert(x0 < x1);
            Babl.Assert(degree is >= MinDegree and <= MaxDegree);
            Babl.Assert(scale is >= MinScale and <= MaxScale);

            return OrthonormalBasis(degree + 1, x0, x1, scale)
                       .GammaProjectCopy(gamma, degree + 1, x0, x1)
                       .Shrink();
        }
    }
}
