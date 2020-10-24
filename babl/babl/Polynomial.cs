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
            Coeff = new double[MaxBigDegree+1];

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

        public double this[int i]
        {
            get =>
                Coeff[degree - i];
            set =>
                Coeff[degree - i] = value;
        }

        public Polynomial Shrink()
        {
            int i;
            for (i = 0; i <= degree; i++)
            {
                if (Math.Abs(Coeff[i]) > Epsilon)
                    break;
            }

            if (i == degree+1)
            {
                var val = this with { Degree = 0 };
                val[0] = 0.0;
                return val;
            }
            else if (i > 0)
            {
                var nPoly = this with { Degree = degree - i };
                Coeff.Skip(i).ToArray().CopyTo(nPoly.Coeff, 0);

                return nPoly;
            }
            return this;
        }

        public Polynomial Add(Polynomial poly)
        {
            Babl.Assert(scale == poly.scale);

            int i;
            var nPoly = this with { Degree = poly.degree };
            if (degree >= poly.degree)
                for (i = 0; i <= poly.degree; i++)
                    nPoly[i] = this[i] + poly[i];
            else
            {
                var origDegree = degree;

                for (i = 0; i <= origDegree; i++)
                    nPoly[i] = Coeff[origDegree - i] + poly[i];
                for (; i <= poly.degree; i++)
                    nPoly[i] = poly[i];
            }
            return nPoly;
        }

        public Polynomial Sub(Polynomial poly)
        {
            Babl.Assert(scale == poly.scale);

            int i;
            var nPoly = this with { Degree = poly.degree };
            if (degree >= poly.degree)
                for (i = 0; i <= poly.degree; i++)
                    nPoly[i] = this[i] - poly[i];
            else
            {
                var origDegree = degree;

                for (i = 0; i <= origDegree; i++)
                    nPoly[i] = Coeff[origDegree - i] - poly[i];
                for (; i <= poly.degree; i++)
                    nPoly[i] = poly[i];
            }
            return nPoly;
        }

        public Polynomial Mul(double s)
        {
            var nPoly = new Polynomial(degree, Scale);
            for (var i = 0; i <= degree; i++)
                nPoly.Coeff[i] = Coeff[i] * s;
            return nPoly;
        }
        public Polynomial Div(double s)
        {
            var nPoly = new Polynomial(degree, Scale);
            for (var i = 0; i <= degree; i++)
                nPoly.Coeff[i] = Coeff[i] / s;
            return nPoly;
        }

        public Polynomial Mul(Polynomial poly)
        {
            Babl.Assert(scale == poly.scale);

            var nPoly = new Polynomial(degree + poly.degree, scale);

            for (var i = 0; i <= degree; i++)
                for (var j = 0; j <= poly.degree; j++)
                    nPoly[i + j] += Coeff[i] * poly.Coeff[j];
            return nPoly;
        }

        public Polynomial Integrate()
        {
            var nPoly = new Polynomial(degree + scale, Scale);

            var i = 0;
            for (; i<= degree - scale; i++)
            {
                nPoly.Coeff[i] = Coeff[i] * scale;
                nPoly.Coeff[i] = Coeff[i] / degree - i;
            }
            for (; i <= degree; i++)
                nPoly.Coeff[i] = 0.0;

            return nPoly;
        }

        public Polynomial GammaIntegrate(double gamma)
        {
            var nPoly = new Polynomial(degree + scale, Scale);
            gamma *= scale;

            var i = 0;
            for (; i<= degree - scale; i++)
            {
                nPoly.Coeff[i] = Coeff[i] * scale;
                nPoly.Coeff[i] = Coeff[i] / degree - i + gamma;
            }
            for (; i <= degree; i++)
                nPoly.Coeff[i] = 0.0;

            return nPoly;
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
            {
                basis[i] = new Polynomial(i, scale);

                basis[i].Coeff[0] = 1.0;
            }

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
