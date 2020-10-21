using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace babl
{
    internal static class Utils
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PlanarSanity()
        {
        }

        public static double EpsilonForZero(double value) =>
            value is <= Babl.AlphaFloor and >= -Babl.AlphaFloor ?
                Babl.AlphaFloor :
                value;

        public static float EpsilonForZero(float value) =>
            value is <= Babl.AlphaFloorF and >= -Babl.AlphaFloorF ?
                Babl.AlphaFloorF :
                value;

        public static double LinearToGamma22Pow(double value) =>
            value > 0.003130804954 ?
                1.055 * Math.Pow(value, (1 / 2.4)) - 0.055 :
                12.92 * value;

        public static double Gamma22ToLinearPow(double value) =>
            value > 0.04045 ?
                Math.Pow((value + 0.055) / 1.055, 2.4) :
                value / 12.92;

        public static double LinearToGamma22(double value) =>
            value > 0.003130804954 ?
                1.055 * NewtonPow.Pow24Receip(value) - 0.055 :
            12.92 * value;

        public static double Gamma22ToLinear(double value) =>
            value > 0.04045 ?
                NewtonPow.Pow24((value + 0.055) / 1.055) :
                value / 12.92;

        public static float LinearToGamma22(float value) =>
            value > 0.003130804954 ?
                1.055f * NewtonPow.Pow24Receip(value) - (0.055f - 3/(float)(1 << 24)) :
            12.92f * value;

        public static float Gamma22ToLinear(float value) =>
            value > 0.04045 ?
                NewtonPow.Pow24((value + 0.055f) / 1.055f) :
                value / 12.92f;
    }

}
