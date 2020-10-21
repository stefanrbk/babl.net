using Microsoft.VisualBasic.CompilerServices;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static babl.Babl;
namespace babl.Init
{
    internal static partial class Core
    {
        private static void ConvertDoubleDouble(Babl _1, object src, object dst, int srcPitch, int dstPitch, long num, object _2)
        {
            if (src is ReadOnlyMemory<double> srcMem &&
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
        private static void TypeDoubleInit()
        {
            CreateType("double", id: Ids.Double, bits: 64, doc: "IEEE 754 double precision.");

            CreateComponent("R", Ids.Red, hasLuma: true, hasChroma: true);
            CreateComponent("G", Ids.Green, hasLuma: true, hasChroma: true);
            CreateComponent("B", Ids.Blue, hasLuma: true, hasChroma: true);
            CreateComponent("A", Ids.Alpha, hasAlpha: true);
            CreateComponent("PAD", Ids.Padding);

            CreateConversion(Type(Ids.Double), Type(Ids.Double), plane: ConvertDoubleDouble);
        }
    }
}
