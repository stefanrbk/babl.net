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
        private static double rint(double value) =>
            Math.Floor(value + 0.5);
        private static void ConvertDoubleU15Scaled(Babl _,
                                                   double minVal,
                                                   double maxVal,
                                                   int min,
                                                   int max,
                                                   object src,
                                                   object dst,
                                                   int srcPitch,
                                                   int dstPitch,
                                                   long num)
        {
            if (src is ReadOnlyMemory<double> srcMem &&
                dst is Memory<ushort> dstMem)
            {
                var srcSpan = srcMem.Span;
                var dstSpan = dstMem.Span;

                while (num-- is > 0)
                {
                    var dval = srcSpan[0];
                    ushort u15val;

                    if (dval < minVal)
                        u15val = (ushort)min;
                    if (dval > maxVal)
                        u15val = (ushort)max;
                    else u15val = (ushort)rint((dval - minVal) / (maxVal - minVal) * (max - min) + min);

                    dstSpan[0] = u15val;
                    dstSpan = dstSpan[dstPitch..];
                    srcSpan = srcSpan[srcPitch..];
                }
            }
        }
        private static void ConvertU15DoubleScaled(Babl _,
                                                   double minVal,
                                                   double maxVal,
                                                   int min,
                                                   int max,
                                                   object src,
                                                   object dst,
                                                   int srcPitch,
                                                   int dstPitch,
                                                   long num)
        {
            if (src is ReadOnlyMemory<ushort> srcMem &&
                dst is Memory<double> dstMem)
            {
                var srcSpan = srcMem.Span;
                var dstSpan = dstMem.Span;

                while (num-- is >0)
                {
                    var u15val = (int)srcSpan[0];
                    double dval;

                    if (u15val < min)
                        dval = minVal;
                    else if (u15val > max)
                        dval = maxVal;
                    else
                        dval = (u15val - min) / (double)(max - min) * (maxVal - minVal) + minVal;

                    dstSpan[0] = dval;
                    dstSpan = dstSpan[dstPitch..];
                    srcSpan = srcSpan[srcPitch..];
                }
            }
        }

        private static void ConvertU15Double(Babl conversion,
                                             object src,
                                             object dst,
                                             int srcPitch,
                                             int dstPitch,
                                             long num,
                                                   object? _) =>
            ConvertU15DoubleScaled(conversion, 0.0, 1.0, 0, (1 << 15), src, dst, srcPitch, dstPitch, num);

        private static void ConvertDoubleU15(Babl conversion,
                                             object src,
                                             object dst,
                                             int srcPitch,
                                             int dstPitch,
                                             long num,
                                                  object? _) =>
            ConvertDoubleU15Scaled(conversion, 0.0, 1.0, 0, (1 << 15), src, dst, srcPitch, dstPitch, num);

        private static void ConvertFloatU15Scaled(Babl _,
                                                  float minVal,
                                                  float maxVal,
                                                  int min,
                                                  int max,
                                                  object src,
                                                  object dst,
                                                  int srcPitch,
                                                  int dstPitch,
                                                  long num)
        {
            if (src is ReadOnlyMemory<float> srcMem &&
                dst is Memory<ushort> dstMem)
            {
                var srcSpan = srcMem.Span;
                var dstSpan = dstMem.Span;

                while (num-- is > 0)
                {
                    var fval = srcSpan[0];
                    ushort u15val;

                    if (fval < minVal)
                        u15val = (ushort)min;
                    if (fval > maxVal)
                        u15val = (ushort)max;
                    else u15val = (ushort)rint((fval - minVal) / (maxVal - minVal) * (max - min) + min);

                    dstSpan[0] = u15val;
                    dstSpan = dstSpan[dstPitch..];
                    srcSpan = srcSpan[srcPitch..];
                }
            }
        }
        private static void ConvertU15FloatScaled(Babl _,
                                                  float minVal,
                                                  float maxVal,
                                                  int min,
                                                  int max,
                                                  object src,
                                                  object dst,
                                                  int srcPitch,
                                                  int dstPitch,
                                                  long num)
        {
            if (src is ReadOnlyMemory<ushort> srcMem &&
                dst is Memory<float> dstMem)
            {
                var srcSpan = srcMem.Span;
                var dstSpan = dstMem.Span;

                while (num-- is > 0)
                {
                    var u15val = (int)srcSpan[0];
                    float dval;

                    if (u15val < min)
                        dval = minVal;
                    else if (u15val > max)
                        dval = maxVal;
                    else
                        dval = (u15val - min) / (float)(max - min) * (maxVal - minVal) + minVal;

                    dstSpan[0] = dval;
                    dstSpan = dstSpan[dstPitch..];
                    srcSpan = srcSpan[srcPitch..];
                }
            }
        }

        private static void ConvertU15Float(Babl conversion,
                                             object src,
                                             object dst,
                                             int srcPitch,
                                             int dstPitch,
                                             long num,
                                             object? _) =>
            ConvertU15FloatScaled(conversion, 0.0f, 1.0f, 0, (1 << 15), src, dst, srcPitch, dstPitch, num);

        private static void ConvertFloatU15(Babl conversion,
                                             object src,
                                             object dst,
                                             int srcPitch,
                                             int dstPitch,
                                             long num,
                                             object? _) =>
            ConvertFloatU15Scaled(conversion, 0.0f, 1.0f, 0, (1 << 15), src, dst, srcPitch, dstPitch, num);

        private static void TypeU15Init()
        {
            logOnNameLookups = false;

            CreateType("u15", bits: 16);

            CreateConversion(Type("u15"), Type(Ids.Double), plane: ConvertU15Double);
            CreateConversion(Type(Ids.Double), Type("u15"), plane: ConvertDoubleU15);
            CreateConversion(Type("u15"), Type(Ids.Float), plane: ConvertU15Float);
            CreateConversion(Type(Ids.Float), Type("u15"), plane: ConvertFloatU15);

            logOnNameLookups = true;
        }
    }
}
