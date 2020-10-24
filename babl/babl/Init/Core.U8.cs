using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using static babl.Babl;
using static babl.Ids;
namespace babl.Init
{
    internal static partial class Core
    {
        private static readonly ConversionRanges<byte, double> U8ByteDouble = new ConversionRanges<byte, double>(0, byte.MaxValue, 0.0, 1.0);
        private static readonly ConversionRanges<double, byte> U8DoubleByte = ~U8ByteDouble;
        private static readonly ConversionRanges<byte, double> U8LumaByteDouble = new ConversionRanges<byte, double>(16, 235, 0.0, 1.0);
        private static readonly ConversionRanges<double, byte> U8LumaDoubleByte = ~U8LumaByteDouble;
        private static readonly ConversionRanges<byte, double> U8ChromaByteDouble = new ConversionRanges<byte, double>(16, 240, -0.5, 0.5);
        private static readonly ConversionRanges<double, byte> U8ChromaDoubleByte = ~U8ChromaByteDouble;
        private static readonly ConversionRanges<byte, float> U8ByteFloat = new ConversionRanges<byte, float>(0, byte.MaxValue, 0.0f, 1.0f);
        private static readonly ConversionRanges<float, byte> U8FloatByte = ~U8ByteFloat;
        private static readonly ConversionRanges<byte, float> U8LumaByteFloat = new ConversionRanges<byte, float>(16, 235, 0.0f, 1.0f);
        private static readonly ConversionRanges<float, byte> U8LumaFloatByte = ~U8LumaByteFloat;
        private static readonly ConversionRanges<byte, float> U8ChromaByteFloat = new ConversionRanges<byte, float>(16, 240, -0.5f, 0.5f);
        private static readonly ConversionRanges<float, byte> U8ChromaFloatByte = ~U8ChromaByteFloat;
        private static void ConvertU8Double(Babl _1, object src, object dst, int srcPitch, int dstPitch,
                                            long num, object? _2) =>
            Convert<byte, double>(src, dst, srcPitch, dstPitch, num, v => v);
        private static void ConvertDoubleU8(Babl _1, object src, object dst, int srcPitch, int dstPitch,
                                               long num, object? _2) =>
            Convert<double, byte>(src, dst, srcPitch, dstPitch, num, v => (byte)v);
        private static void ConvertU8LumaDouble(Babl _1, object src, object dst, int srcPitch, int dstPitch,
                                                long num, object? _2) =>
            Convert<byte, double>(src, dst, srcPitch, dstPitch, num, ScaleU8LumaDouble);
        private static void ConvertDoubleU8Luma(Babl _1, object src, object dst, int srcPitch, int dstPitch,
                                                long num, object? _2) =>
            Convert<double, byte>(src, dst, srcPitch, dstPitch, num, ScaleDoubleU8Luma);
        private static void ConvertU8ChromaDouble(Babl _1, object src, object dst, int srcPitch, int dstPitch,
                                                long num, object? _2) =>
            Convert<byte, double>(src, dst, srcPitch, dstPitch, num, ScaleU8ChromaDouble);
        private static void ConvertDoubleU8Chroma(Babl _1, object src, object dst, int srcPitch, int dstPitch,
                                                long num, object? _2) =>
            Convert<double, byte>(src, dst, srcPitch, dstPitch, num, ScaleDoubleU8Chroma);

        private static void ConvertU8Float(Babl _1, object src, object dst, int srcPitch, int dstPitch,
                                            long num, object? _2) =>
            Convert<byte, float>(src, dst, srcPitch, dstPitch, num, v => v);
        private static void ConvertFloatU8(Babl _1, object src, object dst, int srcPitch, int dstPitch,
                                               long num, object? _2) =>
            Convert<float, byte>(src, dst, srcPitch, dstPitch, num, v => (byte)v);
        private static void ConvertU8LumaFloat(Babl _1, object src, object dst, int srcPitch, int dstPitch,
                                                long num, object? _2) =>
            Convert<byte, float>(src, dst, srcPitch, dstPitch, num, ScaleU8LumaFloat);
        private static void ConvertFloatU8Luma(Babl _1, object src, object dst, int srcPitch, int dstPitch,
                                                long num, object? _2) =>
            Convert<float, byte>(src, dst, srcPitch, dstPitch, num, ScaleFloatU8Luma);
        private static void ConvertU8ChromaFloat(Babl _1, object src, object dst, int srcPitch, int dstPitch,
                                                long num, object? _2) =>
            Convert<byte, float>(src, dst, srcPitch, dstPitch, num, ScaleU8ChromaFloat);
        private static void ConvertFloatU8Chroma(Babl _1, object src, object dst, int srcPitch, int dstPitch,
                                                long num, object? _2) =>
            Convert<float, byte>(src, dst, srcPitch, dstPitch, num, ScaleFloatU8Chroma);

        private static double ScaleU8LumaDouble(byte value) =>
            Scale(value, U8LumaByteDouble, v => Lerp(v, U8LumaByteDouble.ToDouble));
        private static byte ScaleDoubleU8Luma(double value) =>
            Scale(value, U8LumaDoubleByte, v => (byte)LerpClampPrepared(v, U8LumaDoubleByte.ToDouble));
        private static double ScaleU8ChromaDouble(byte value) =>
            Scale(value, U8ChromaByteDouble, v => Lerp(v, U8ChromaByteDouble.ToDouble));
        private static byte ScaleDoubleU8Chroma(double value) =>
            Scale(value, U8ChromaDoubleByte, v => (byte)LerpClampPrepared(v, U8ChromaDoubleByte.ToDouble));

        private static float ScaleU8LumaFloat(byte value) =>
            Scale(value, U8LumaByteFloat, v => Lerp(v, U8LumaByteFloat.ToFloat));
        private static byte ScaleFloatU8Luma(float value) =>
            Scale(value, U8LumaFloatByte, v => (byte)LerpClampPrepared(v, U8LumaFloatByte.ToFloat));
        private static float ScaleU8ChromaFloat(byte value) =>
            Scale(value, U8ChromaByteFloat, v => Lerp(v, U8ChromaByteFloat.ToFloat));
        private static byte ScaleFloatU8Chroma(float value) =>
            Scale(value, U8ChromaFloatByte, v => (byte)LerpClampPrepared(v, U8ChromaFloatByte.ToFloat));
        private static void TypeU8Init()
        {
            var u8Type = CreateType("u8", id: U8, bits: 8, doc: "byte, 8 bit unsigned integer, values from 0-255");
            var u8LumaType = CreateType("u8-luma", id: U8Luma, bits: 8, doc: "8 bit unsigned integer, valuers from 16-235");
            var u8ChromaType = CreateType("u8-chroma", id: U8Chroma, integer: true, unsigned: true, bits: 8, min: 16L, max: 240L, minVal: -0.5, maxVal: 0.5, doc: "8 bit unsigned integer -0.5 to 0.5 maps to 16-240");
            var doubleType = Type(Ids.Double);
            var floatType = Type(Float);

            // from u8
            CreateConversion(u8Type, doubleType, plane: ConvertU8Double);
            CreateConversion(u8Type, floatType, plane: ConvertU8Float);
            // to u8
            CreateConversion(doubleType, u8Type, plane: ConvertDoubleU8);
            CreateConversion(floatType, u8Type, plane: ConvertDoubleU8);
            // from u8luma
            CreateConversion(u8LumaType, doubleType, plane: ConvertU8LumaDouble);
            CreateConversion(u8LumaType, floatType, plane: ConvertU8LumaFloat);
            // to u8luma
            CreateConversion(doubleType, u8LumaType, plane: ConvertDoubleU8Luma);
            CreateConversion(floatType, u8LumaType, plane: ConvertFloatU8Luma);
            // from u8chroma
            CreateConversion(u8ChromaType, doubleType, plane: ConvertU8ChromaDouble);
            CreateConversion(u8ChromaType, floatType, plane: ConvertU8ChromaFloat);
            // to u8chroma
            CreateConversion(doubleType, u8ChromaType, plane: ConvertDoubleU8Chroma);
            CreateConversion(floatType, u8ChromaType, plane: ConvertFloatU8Chroma);
        }
    }
}
