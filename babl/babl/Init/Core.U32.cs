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
        private static readonly ConversionRanges<uint, double> U32UintDouble = new ConversionRanges<uint, double>(0, uint.MaxValue, 0.0, 1.0);
        private static readonly ConversionRanges<uint, float> U32UintFloat = new ConversionRanges<uint, float>(0, uint.MaxValue, 0.0f, 1.0f);
        private static readonly ConversionRanges<double, uint> U32DoubleUint = ~U32UintDouble;
        private static readonly ConversionRanges<float, uint> U32FloatUint = ~U32UintFloat;

        private static void ConvertU32Double(Babl _1, object src, object dst, int srcPitch, int dstPitch,
                                             long num, object? _2) =>
                Convert<uint, double>(src, dst, srcPitch, dstPitch, num, ScaleU32Double);

        private static void ConvertDoubleU32(Babl _1, object src, object dst, int srcPitch, int dstPitch,
                                             long num, object? _2) =>
            Convert<double, uint>(src, dst, srcPitch, dstPitch, num, ScaleDoubleU32);

        private static void ConvertU32Float(Babl _1, object src, object dst, int srcPitch, int dstPitch,
                                            long num, object? _2) =>
                Convert<uint, float>(src, dst, srcPitch, dstPitch, num, ScaleU32Float);

        private static void ConvertFloatU32(Babl _1, object src, object dst, int srcPitch, int dstPitch,
                                            long num, object? _2) =>
            Convert<float, uint>(src, dst, srcPitch, dstPitch, num, ScaleFloatU32);

        private static double ScaleU32Double(uint value) =>
            Scale(value, U32UintDouble, v => Lerp(v, U32UintDouble.ToDouble));
        private static uint ScaleDoubleU32(double value) =>
            Scale(value, U32DoubleUint, v => (uint)LerpClampPrepared(v, U32DoubleUint.ToDouble));

        private static float ScaleU32Float(uint value) =>
            Scale(value, U32UintFloat, v => Lerp(v, U32UintFloat.ToFloat));
        private static uint ScaleFloatU32(float value) =>
            Scale(value, U32FloatUint, v => (uint)LerpClampPrepared(v, U32FloatUint.ToFloat));
        private static void TypeU32Init()
        {
            var u32Type = CreateType("u32", id: U32, bits: 32);
            var doubleType = Type(Ids.Double);
            var floatType = Type(Float);

            CreateConversion(u32Type, doubleType, plane: ConvertU32Double);
            CreateConversion(doubleType, u32Type, plane: ConvertDoubleU32);

            CreateConversion(u32Type, floatType, plane: ConvertU32Float);
            CreateConversion(floatType, u32Type, plane: ConvertFloatU32);
        }
    }
}
