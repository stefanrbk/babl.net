using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace babl.Init
{
    internal static partial class Core
    {
        public static void Init()
        {
            Babl.logOnNameLookups = true;

            InitTypes();

            Babl.logOnNameLookups = false;
        }

        private static void InitTypes()
        {
            TypeDoubleInit();
            TypeFloatInit();
            TypeU15Init();
            TypeHalfInit();
            TypeU8Init();
            TypeU16Init();
            TypeU32Init();
        }

        private static bool IsValidSrcDst<Tsrc, Tdst>(object src, object dst) =>
            src is ReadOnlyMemory<Tsrc> &&
            dst is Memory<Tdst>;
        private static ReadOnlySpan<T> SrcObjectToSpan<T>(object obj) =>
            ((ReadOnlyMemory<T>)obj).Span;
        private static Span<T> DstObjectToSpan<T>(object obj) =>
            ((Memory<T>)obj).Span;
        private static void Convert<Tsrc, Tdst>(object src, object dst, int srcPitch, int dstPitch, long num, Func<Tsrc, Tdst> conversionFunc)
        {
            if (IsValidSrcDst<Tsrc, Tdst>(src, dst))
            {
                var srcSpan = SrcObjectToSpan<Tsrc>(src);
                var dstSpan = DstObjectToSpan<Tdst>(dst);

                while (num-- is > 0)
                {
                    dstSpan[0] = conversionFunc(srcSpan[0]);
                    dstSpan = dstSpan[dstPitch..];
                    srcSpan = srcSpan[srcPitch..];
                }
            }
        }

        private static void Copy<T>(Babl _1, object src, object dst, int srcPitch, int dstPitch,
                                    long num, object? _2) =>
            Convert<T, T>(src, dst, srcPitch, dstPitch, num, v => v);
    }
}
