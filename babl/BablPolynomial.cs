using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using static babl.Babl;

namespace babl
{
    delegate double EvalFunc(in BablPolynomial poly, double x);

    [SkipLocalsInit]
    unsafe struct BablPolynomial
    {
        internal const int MinDegree = 0;
        internal const int MaxDegree = 10;
        internal const int MinScale = 1;
        internal const int MaxScale = 2;
        internal const double Epsilon = 1e-10;

        int degree;
        int scale;
        fixed double coeff[MaxDegree + 1];

        public static double Eval(BablPolynomial* poly, double x)
        {
            Assert(poly->scale is >= MinScale and <= MaxScale);

            BablBigPolynomial* bigPoly = (BablBigPolynomial*)poly;

            if (poly->scale == 1)
            {
                var x2 = x * x;

                return Eval(bigPoly, bigPoly->degree, x2) +
                      (Eval(bigPoly, bigPoly->degree - 1, x2) * x);
            }
            else
            {
                return Eval(bigPoly, bigPoly->degree, x) +
                      (Eval(bigPoly, bigPoly->degree - 1, x) * Math.Sqrt(x));
            }
        }

        private static double Eval(BablBigPolynomial* poly, int degree, double x) =>
            degree switch
            {
                >= 2 => (Eval(poly, degree - 2, x) * x) + poly->coeff[degree],
                0 or 1 => poly->coeff[degree],
                _ => 0
            };

        public static void SetDegree(BablPolynomial* poly, int degree, int scale)
        {
            Assert(degree >= MinDegree);
            Assert(degree <= BablBigPolynomial.MaxDegree);
            Assert(scale >= MinScale);
            Assert(scale <= MaxScale);

            poly->degree = degree;
            poly->scale = scale;
        }

        public double this[int i]
        {
            get => coeff[degree - i];
            set => coeff[degree - i] = value; 
        }

        public static void Copy(BablPolynomial* poly, in BablPolynomial* rpoly)
        {
            poly->degree = rpoly->degree;
            poly->scale = rpoly->scale;

            Unsafe.CopyBlock(poly->coeff, rpoly->coeff, (MaxDegree + 1) * sizeof(double));
        }

        public static void Reset(BablPolynomial* poly, int scale)
        {
            SetDegree(poly, 0, scale);
            (*poly)[0] = 0.0;
        }

        public static void Shrink(BablPolynomial* poly)
        {
            int i;

            for (i = 0; i <= poly->degree; i++)
                if (Math.Abs(poly->coeff[i]) > Epsilon)
                    break;

            if (i == poly->degree + 1)
                Reset(poly, poly->scale);
            else if (i > 0)
            {
                Unsafe.CopyBlock(poly->coeff, &poly->coeff[i], (uint)((poly->degree - i + 1) * sizeof(double)));

                SetDegree(poly, poly->degree - i, poly->scale);
            }
        }

        public static void Add(BablPolynomial* poly, in BablPolynomial* rpoly)
        {
            int i;

            Assert(poly->scale == rpoly->scale);

            if (poly->degree >= rpoly->degree)
                for (i = 0; i <= rpoly->degree; i++)
                    (*poly)[i] += (*rpoly)[i];
            else
            {
                var orgDegree = poly->degree;

                SetDegree(poly, rpoly->degree, poly->scale);

                for (i = 0; i <= orgDegree; i++)
                    (*poly)[i] = poly->coeff[orgDegree - i] + (*rpoly)[i];

                for (; i <= rpoly->degree; i++)
                    (*poly)[i] = (*rpoly)[i];
            }
        }

        public static void Subtract(BablPolynomial* poly, in BablPolynomial* rpoly)
        {
            int i;

            Assert(poly->scale == rpoly->scale);

            if (poly->degree >= rpoly->degree)
                for (i = 0; i <= rpoly->degree; i++)
                    (*poly)[i] -= (*rpoly)[i];
            else
            {
                var orgDegree = poly->degree;

                SetDegree(poly, rpoly->degree, poly->scale);

                for (i = 0; i <= orgDegree; i++)
                    (*poly)[i] = poly->coeff[orgDegree - i] - (*rpoly)[i];

                for (; i <= rpoly->degree; i++)
                    (*poly)[i] = -(*rpoly)[i];
            }
        }

        public static void ScalarMultiply(BablPolynomial* poly, double a)
        {
            for (var i = 0; i <= poly->degree; i++)
                (*poly)[i] *= a;
        }

        public static void ScalarDivide(BablPolynomial* poly, double a)
        {
            for (var i = 0; i <= poly->degree; i++)
                (*poly)[i] /= a;
        }

        public static void MultiplyCopy(BablPolynomial* poly, in BablPolynomial* poly1, in BablPolynomial* poly2)
        {
            int i, j;

            Assert(poly1->scale == poly2->scale);

            SetDegree(poly, poly1->degree + poly2->degree, poly1->scale);

            Unsafe.InitBlock(poly->coeff, 0, (uint)((poly->degree + 1) * sizeof(double)));

            for (i = poly1->degree; i >= 0; i--)
                for (j = poly2->degree; j >= 0; j--)
                    (*poly)[i + j] += (*poly1)[i] * (*poly2)[j];
        }

        public static void Integrate(BablPolynomial* poly)
        {
            int i;

            SetDegree(poly, poly->degree + poly->scale, poly->scale);

            for (i = 0; i <= poly->degree - poly->scale; i++)
            {
                poly->coeff[i] *= poly->scale;
                poly->coeff[i] /= poly->degree - i;
            }

            for (; i <= poly->degree; i++)
                poly->coeff[i] = 0;
        }

        public static void GammaIntegrate(BablPolynomial* poly, double gamma)
        {
            int i;

            SetDegree(poly, poly->degree + poly->scale, poly->scale);

            gamma *= poly->scale;

            for (i = 0; i <= poly->degree - poly->scale; i++)
            {
                poly->coeff[i] *= poly->scale;
                poly->coeff[i] /= poly->degree - i + gamma;
            }

            for (; i <= poly->degree; i++)
                poly->coeff[i] = 0;
        }
                
        public static double InnerProduct(in BablPolynomial* poly1,
                                          in BablPolynomial* poly2,
                                          double x0,
                                          double x1)
        {
            Unsafe.SkipInit<BablBigPolynomial>(out var temp);

            MultiplyCopy((BablPolynomial*)&temp, poly1, poly2);
            Integrate((BablPolynomial*)&temp);

            return Eval((BablPolynomial*)&temp, x1) -
                   Eval((BablPolynomial*)&temp, x0);
        }

        public static double GammaInnerProduct(in BablPolynomial* poly,
                                               double gamma,
                                               double x0,
                                               double x1)
        {
            Unsafe.SkipInit<BablBigPolynomial>(out var temp);

            Copy((BablPolynomial*)&temp, poly);
            GammaIntegrate((BablPolynomial*)&temp, gamma);

            return (Eval((BablPolynomial*)&temp, x1) * Math.Pow(x1, gamma)) -
                   (Eval((BablPolynomial*)&temp, x0) * Math.Pow(x0, gamma));
        }

        public static double Normal(in BablPolynomial* poly, double x0, double x1) =>
            Math.Sqrt(InnerProduct(poly, poly, x0, x1));

        public static void Normalize(in BablPolynomial* poly, double x0, double x1)
        {
            var norm = Normal(poly, x0, x1);

            if (norm > Epsilon)
                ScalarDivide(poly, norm);
        }

        public static void ProjectCopy(BablPolynomial* poly,
                                       in BablPolynomial* rpoly,
                                       in BablPolynomial[] basis,
                                       double x0,
                                       double x1)
        {
            Assert(basis.Length > 0);

            fixed (BablPolynomial* b = basis)
            {
                Reset(poly, b[0].scale);

                for (var i = 0; i < basis.Length; i++)
                {
                    Unsafe.SkipInit<BablPolynomial>(out var temp);

                    Copy(&temp, &b[i]);
                    ScalarMultiply(&temp, InnerProduct(&temp, rpoly, x0, x1));
                    Add(poly, &temp);
                }
            }
        }

        public static void GammaProjectCopy(BablPolynomial* poly,
                                            double gamma,
                                            in BablPolynomial[] basis,
                                            double x0,
                                            double x1)
        {
            Assert(basis.Length > 0);

            fixed (BablPolynomial* b = basis)
            {
                Reset(poly, b[0].scale);

                for (var i = 0; i < basis.Length; i++)
                {
                    Unsafe.SkipInit<BablPolynomial>(out var temp);

                    Copy(&temp, &b[i]);
                    ScalarMultiply(&temp, GammaInnerProduct(&temp, gamma, x0, x1));
                    Add(poly, &temp);
                }
            }
        }

        public static void GramSchmidt(BablPolynomial[] basis,
                                       double x0,
                                       double x1)
        {
            fixed (BablPolynomial* b = basis)
                for (var i = 0; i < basis.Length; i++)
                {
                    if (i > 0)
                    {
                        Unsafe.SkipInit<BablPolynomial>(out var temp);

                        ProjectCopy(&temp, &b[i], basis, x0, x1);
                        Subtract(&b[i], &temp);
                    }
                    Normalize(&b[i], x0, x1);
                }
        }

        public static void Basis(BablPolynomial[] basis, int scale)
        {
            fixed(BablPolynomial* b = basis)
                for(var i = 0; i < basis.Length; i++)
                {
                    SetDegree(&b[i], i, scale);

                    b[i].coeff[0] = 1.0;
                    Unsafe.InitBlock(&b[i].coeff[1], 0, (uint)(i * sizeof(double)));
                }
        }

        public static void OrthonormalBasis(BablPolynomial[] basis,
                                            double x0,
                                            double x1,
                                            int scale)
        {
            Basis(basis, scale);
            GramSchmidt(basis, x0, x1);
        }

        public static void ApproximateGamma(BablPolynomial* poly,
                                            double gamma,
                                            double x0,
                                            double x1,
                                            int degree,
                                            int scale)
        {
            Assert(poly is not null);
            Assert(gamma >= 0.0);
            Assert(x0 >= 0.0);
            Assert(x0 < x1);
            Assert(degree is >= MinDegree and <= MaxDegree);
            Assert(scale is >= MinScale and <= MaxScale);

            var basis = new BablPolynomial[degree];

            OrthonormalBasis(basis, x0, x1, scale);

            GammaProjectCopy(poly, gamma, basis, x0, x1);
            Shrink(poly);
        }

        unsafe struct BablBigPolynomial
        {
            public const int MaxDegree = (2 * BablPolynomial.MaxDegree) + MaxScale;
            public int degree;
            public int scale;
            public fixed double coeff[MaxDegree + 1];
        }
    }
}
