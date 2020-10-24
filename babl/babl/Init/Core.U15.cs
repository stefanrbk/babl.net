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
        private static readonly ushort U15UshortMax = 32768;
        private static readonly ConversionRanges<ushort, double> U15UshortDouble = new ConversionRanges<ushort, double>(0, U15UshortMax, 0.0, 1.0);
        private static readonly ConversionRanges<ushort, float> U15UshortFloat = new ConversionRanges<ushort, float>(0, U15UshortMax, 0.0f, 1.0f);
        private static readonly ConversionRanges<double, ushort> U15DoubleUshort = ~U15UshortDouble;
        private static readonly ConversionRanges<float, ushort> U15FloatUshort = ~U15UshortFloat;

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
            Scale(value, U15UshortDouble, v => Lerp(v, U15UshortDouble.ToDouble));
        private static ushort ScaleDoubleU15(double value) =>
            Scale(value, U15DoubleUshort, v => (ushort)LerpClampPrepared(v, U15DoubleUshort.ToDouble));

        private static float ScaleU15Float(ushort value) =>
            Scale(value, U15UshortFloat, v => Lerp(v, U15UshortFloat.ToFloat));
        private static ushort ScaleFloatU15(float value) =>
            Scale(value, U15FloatUshort, v => (ushort)LerpClampPrepared(v, U15FloatUshort.ToFloat));

        private static void TypeU15Init()
        {
            logOnNameLookups = false;

            var u15Type = CreateType("u15", bits: 16);
            var doubleType = Type(Ids.Double);
            var floatType = Type(Float);

            CreateConversion(u15Type, doubleType, plane: ConvertU15Double);
            CreateConversion(doubleType, u15Type, plane: ConvertDoubleU15);
            CreateConversion(u15Type, floatType, plane: ConvertU15Float);
            CreateConversion(floatType, u15Type, plane: ConvertFloatU15);

            logOnNameLookups = true;
        }
    }
}
