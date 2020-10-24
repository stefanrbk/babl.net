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
        private static void ConvertHalfDouble(Babl _1, object src, object dst, int srcPitch, int dstPitch,
                                               long num, object? _2) =>
            Convert<Half, double>(src, dst, srcPitch, dstPitch, num, v => (double)v);
        private static void ConvertDoubleHalf(Babl _1, object src, object dst, int srcPitch, int dstPitch,
                                               long num, object? _2) =>
            Convert<double, Half>(src, dst, srcPitch, dstPitch, num, v => (Half)v);

        private static void ConvertHalfFloat(Babl _1, object src, object dst, int srcPitch, int dstPitch,
                                               long num, object? _2) =>
            Convert<Half, float>(src, dst, srcPitch, dstPitch, num, v => (float)v);
        private static void ConvertFloatHalf(Babl _1, object src, object dst, int srcPitch, int dstPitch,
                                               long num, object? _2) =>
            Convert<float, Half>(src, dst, srcPitch, dstPitch, num, v => (Half)v);

        private static void TypeHalfInit()
        {
            var halfType = CreateType("half", id: Ids.Half, bits: 16, doc: "IEEE 754 half precision.");
            var doubleType = Type(Ids.Double);
            var floatType = Type(Float);

            CreateConversion(halfType, doubleType, plane: ConvertHalfDouble);
            CreateConversion(doubleType, halfType, plane: ConvertDoubleHalf);
            CreateConversion(halfType, floatType, plane: ConvertHalfFloat);
            CreateConversion(floatType, halfType, plane: ConvertFloatHalf);
        }
    }
}
