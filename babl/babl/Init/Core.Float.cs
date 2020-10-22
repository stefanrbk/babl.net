﻿using System;
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
        private static void ConvertFloatDouble(Babl _1, object src, object dst, int srcPitch, int dstPitch,
                                               long num, object? _2) =>
            Convert<float, double>(src, dst, srcPitch, dstPitch, num, v => v);
        private static void ConvertDoubleFloat(Babl _1, object src, object dst, int srcPitch, int dstPitch,
                                               long num, object? _2) =>
            Convert<double, float>(src, dst, srcPitch, dstPitch, num, v => (float)v);

        private static void TypeFloatInit()
        {
            CreateType("float", id: Float, bits: 32, doc: "IEEE 754 single precision");

            CreateConversion(Type(Float), Type(Ids.Double), plane: ConvertFloatDouble);
            CreateConversion(Type(Ids.Double), Type(Float), plane: ConvertDoubleFloat);
            CreateConversion(Type(Float), Type(Float), plane: Copy<float>);

        }
    }
}
