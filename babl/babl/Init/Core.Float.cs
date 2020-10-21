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
        private static void ConvertDoubleFloat(Babl _1, object src, object dst, int srcPitch, int dstPitch, long num, object _2)
        {
            if (src is ReadOnlyMemory<double> srcMem &&
                dst is Memory<float> dstMem)
            {
                var srcSpan = srcMem.Span;
                var dstSpan = dstMem.Span;

                while (num-- is > 0)
                {
                    dstSpan[0] = (float)srcSpan[0];
                    dstSpan = dstSpan[dstPitch..];
                    srcSpan = srcSpan[srcPitch..];
                }
            }
        }
        private static void ConvertFloatDouble(Babl _1, object src, object dst, int srcPitch, int dstPitch, long num, object _2)
        {
            if (src is ReadOnlyMemory<float> srcMem &&
                dst is Memory<double> dstMem)
            {
                var srcSpan = srcMem.Span;
                var dstSpan = dstMem.Span;

                while (num-- is > 0)
                {
                    dstSpan[0] = srcSpan[0];
                    dstSpan = dstSpan[dstPitch..];
                    srcSpan = srcSpan[srcPitch..];
                }
            }
        }
        private static void ConvertFloatFloat(Babl _1, object src, object dst, int srcPitch, int dstPitch, long num, object _2)
        {
            if (src is ReadOnlyMemory<float> srcMem &&
                dst is Memory<float> dstMem)
            {
                var srcSpan = srcMem.Span;
                var dstSpan = dstMem.Span;

                while (num-- is > 0)
                {
                    dstSpan[0] = srcSpan[0];
                    dstSpan = dstSpan[dstPitch..];
                    srcSpan = srcSpan[srcPitch..];
                }
            }
        }
        private static void TypeFloatInit()
        {
            CreateType("float", id: Float, bits: 32, doc: "IEEE 754 single precision");

            CreateConversion(Conversion(Float), Conversion(Ids.Double), plane: ConvertFloatDouble);
            CreateConversion(Conversion(Ids.Double), Conversion(Float), plane: ConvertDoubleFloat);
            CreateConversion(Conversion(Float), Conversion(Float), plane: ConvertFloatFloat);

        }
    }
}
