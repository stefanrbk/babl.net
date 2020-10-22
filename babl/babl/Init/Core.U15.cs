using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static babl.Babl;
using static babl.Ids;
namespace babl.Init
{
    internal static partial class Core
    {
        private const ushort U15MaxUshort = 32768;
        private const ushort U15MinUshort = 0;
        private const double U15MaxDouble = 1.0;
        private const double U15MinDouble = 0.0;
        private const float U15MaxFloat = 1.0f;
        private const float U15MinFloat = 0.0f;
        private static Tdst Scale<Tsrc, Tdst>(Tsrc value,
                                              Tsrc minSrc,
                                              Tsrc maxSrc,
                                              Tdst minDst,
                                              Tdst maxDst,
                                              Func<Tsrc, Tdst> conversion) where Tsrc : IComparable
                                                                           where Tdst : IComparable
        {
            if (value.CompareTo(minSrc) < 0)
                return minDst;
            if (value.CompareTo(maxSrc) > 0)
                return maxDst;
            return conversion(value);
        }
        private static double rint(double value) =>
            Math.Floor(value + 0.5);

        private static void ConvertU15Double(Babl _1, object src, object dst, int srcPitch, int dstPitch, 
                                             long num, object? _2) =>
                Convert<ushort, double>(src, dst, srcPitch, dstPitch, num, ScaleU15Double);

        private static void ConvertDoubleU15(Babl _1, object src, object dst, int srcPitch, int dstPitch,
                                             long num, object? _2) =>
            Convert<double, ushort>(src, dst, srcPitch, dstPitch, num, ScaleDoubleU15);

        private static void ConvertU15Float(Babl _1, object src, object dst, int srcPitch, int dstPitch,
                                            long num, object? _2) =>
                Convert<ushort, float>(src, dst, srcPitch, dstPitch, num, ScaleU15Float);

        private static void ConvertFloatU15(Babl _1, object src, object dst, int srcPitch, int dstPitch,
                                            long num,object? _2) =>
            Convert<float, ushort>(src, dst, srcPitch, dstPitch, num, ScaleFloatU15);

        private static double ScaleU15Double(ushort value) =>
            Scale(value, U15MinUshort, U15MaxUshort, U15MinDouble, U15MaxDouble, v => v / (double)U15MaxUshort);
        private static ushort ScaleDoubleU15(double value) =>
            Scale(value, U15MinDouble, U15MaxDouble, U15MinUshort, U15MaxUshort, v => (ushort)rint(v * U15MaxUshort));

        private static float ScaleU15Float(ushort value) =>
            Scale(value, U15MinUshort, U15MaxUshort, U15MinFloat, U15MaxFloat, v => v / (float)U15MaxUshort);
        private static ushort ScaleFloatU15(float value) =>
            Scale(value, U15MinFloat, U15MaxFloat, U15MinUshort, U15MaxUshort, v => (ushort)rint(v * U15MaxUshort));

        private static void TypeU15Init()
        {
            logOnNameLookups = false;

            CreateType("u15", bits: 16);

            CreateConversion(Type("u15"), Type(Ids.Double), plane: ConvertU15Double);
            CreateConversion(Type(Ids.Double), Type("u15"), plane: ConvertDoubleU15);
            CreateConversion(Type("u15"), Type(Float), plane: ConvertU15Float);
            CreateConversion(Type(Float), Type("u15"), plane: ConvertFloatU15);

            logOnNameLookups = true;
        }
    }
}
