using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace babl
{
    static class Util
    {
        public static double LinearToGamma2dot2(double value) =>
            Math.Pow(value, 1 / 2.2);

        public static double Gamma2dot2ToLinear(double value) =>
            Math.Pow(value, 2.2);
        public static float LinearToGamma2dot2(float value) =>
            MathF.Pow(value, 1 / 2.2f);

        public static float Gamma2dot2ToLinear(float value) =>
            MathF.Pow(value, 2.2f);
    }
}
