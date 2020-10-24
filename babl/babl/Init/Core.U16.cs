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
        private static readonly ConversionRanges<ushort, double> U16UshortDouble = new ConversionRanges<ushort, double>(0, ushort.MaxValue, 0.0, 1.0);
        private static readonly ConversionRanges<ushort, float> U16UshortFloat = new ConversionRanges<ushort, float>(0, ushort.MaxValue, 0.0f, 1.0f);
        private static readonly ConversionRanges<double, ushort> U16DoubleUshort = ~U16UshortDouble;
        private static readonly ConversionRanges<float, ushort> U16FloatUshort = ~U16UshortFloat;

        private static void ConvertU16Double(Babl _1, object src, object dst, int srcPitch, int dstPitch,
                                             long num, object? _2) =>
                Convert<ushort, double>(src, dst, srcPitch, dstPitch, num, ScaleU16Double);

        private static void ConvertDoubleU16(Babl _1, object src, object dst, int srcPitch, int dstPitch,
                                             long num, object? _2) =>
            Convert<double, ushort>(src, dst, srcPitch, dstPitch, num, ScaleDoubleU16);

        private static void ConvertU16Float(Babl _1, object src, object dst, int srcPitch, int dstPitch,
                                            long num, object? _2) =>
                Convert<ushort, float>(src, dst, srcPitch, dstPitch, num, ScaleU16Float);

        private static void ConvertFloatU16(Babl _1, object src, object dst, int srcPitch, int dstPitch,
                                            long num, object? _2) =>
            Convert<float, ushort>(src, dst, srcPitch, dstPitch, num, ScaleFloatU16);

        private static double ScaleU16Double(ushort value) =>
            Scale(value, U16UshortDouble, v => Lerp(v, U16UshortDouble.ToDouble));
        private static ushort ScaleDoubleU16(double value) =>
            Scale(value, U16DoubleUshort, v => (ushort)LerpClampPrepared(v, U16DoubleUshort.ToDouble));

        private static float ScaleU16Float(ushort value) =>
            Scale(value, U16UshortFloat, v => Lerp(v, U16UshortFloat.ToFloat));
        private static ushort ScaleFloatU16(float value) =>
            Scale(value, U16FloatUshort, v => (ushort)LerpClampPrepared(v, U16FloatUshort.ToFloat));

        private static void TypeU16Init()
        {
            var u16Type = CreateType("u16", id: U16, bits: 16);
            var doubleType = Type(Ids.Double);
            var floatType = Type(Float);

            CreateConversion(u16Type, doubleType, plane: ConvertU16Double);
            CreateConversion(doubleType, u16Type, plane: ConvertDoubleU16);

            CreateConversion(u16Type, floatType, plane: ConvertU16Float);
            CreateConversion(floatType, u16Type, plane: ConvertFloatU16);
        }
    }
}
