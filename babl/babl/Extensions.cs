using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static babl.Polynomial;
namespace babl
{
    internal static class Extensions
    {
        public static Polynomial ProjectCopy(this Polynomial[] basis, Polynomial rpoly, int n, double x0, double x1)
        {
            Babl.Assert(n > 0);
            var poly = Empty with { Scale = basis[0].Scale };

            for (var i = 0; i < n; i++)
            {
                var temp = basis[i] with
                {
                    Degree = basis[i].Degree,
                    Scale = basis[i].Scale,
                    Coeff = basis[i].Coeff
                };
                temp.Mul(temp.InnerProduct(rpoly, x0, x1));

                poly.Add(temp);
            }

            return poly;
        }

        public static Polynomial GammaProjectCopy(this Polynomial[] basis, double gamma, int n, double x0, double x1)
        {
            Babl.Assert(n > 0);

            var poly = Empty with { Scale = basis[0].Scale };

            for (var i = 0; i < n; i++)
            {
                var temp = basis[i] with
                {
                    Degree = basis[i].Degree,
                    Scale = basis[i].Scale,
                    Coeff = basis[i].Coeff
                };
                temp.Mul(temp.GammaInnerProduct(gamma, x0, x1));

                poly.Add(temp);
            }

            return poly;
        }
        public static Polynomial[] GramSchmidt(this Polynomial[] basis, int n, double x0, double x1)
        {
            for (var i = 0; i < n; i++)
            {
                if (i > 0)
                {
                    var temp = basis.ProjectCopy(basis[i], i, x0, x1);
                    basis[i] = basis[i].Sub(temp);
                }
                basis[i] = basis[i].Normalize(x0, x1);
            }

            return basis;
        }
    }
}
