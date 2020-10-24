using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

        private static Tdst Scale<Tsrc, Tdst>(Tsrc value,
                                              ConversionRanges<Tsrc, Tdst> cons,
                                              Func<Tsrc, Tdst> conversion) where Tsrc : IComparable, IConvertible
                                                                           where Tdst : IComparable, IConvertible
        {
            if (value.CompareTo(cons.Src.Min) < 0)
                return cons.Dst.Min;
            if (value.CompareTo(cons.Src.Max) > 0)
                return cons.Dst.Max;
            return conversion(value);
        }

        private static double Lerp(double value, ConversionRanges<double, double> cons) =>
            (value - cons.Src.Min) / cons.Src.Sub() * cons.Dst.Sub() + cons.Dst.Min;
        private static float Lerp(float value, ConversionRanges<float, float> cons) =>
            (value - cons.Src.Min) / cons.Src.Sub() * cons.Dst.Sub() + cons.Dst.Min;
        private static double LerpClampPrepared(double value, ConversionRanges<double, double> cons) =>
            Lerp(value, cons) + 0.5;
        private static float LerpClampPrepared(float value, ConversionRanges<float, float> cons) =>
            Lerp(value, cons) + 0.5f;
    }

    internal record Range<T>(T Min, T Max) where T : IConvertible
    {
        public Range<double> ToDouble =>
            new Range<double>(Min.ToDouble(null), Max.ToDouble(null));
        public Range<float> ToFloat =>
            new Range<float>(Min.ToSingle(null), Max.ToSingle(null));
    }

    internal static class Extensions
    {
        public static double Add(this Range<double> value) =>
            value.Max + value.Min;
        public static float Add(this Range<float> value) =>
            value.Max + value.Min;
        public static double Sub(this Range<double> value) =>
            value.Max - value.Min;
        public static float Sub(this Range<float> value) =>
            value.Max - value.Min;
    }
    internal record ConversionRanges<Tsrc, Tdst>(Range<Tsrc> Src, Range<Tdst> Dst) where Tsrc: IConvertible
                                                                                    where Tdst: IConvertible
    {
        public ConversionRanges(Tsrc sMin, Tsrc sMax, Tdst dMin, Tdst dMax)
            : this(new Range<Tsrc>(sMin, sMax), new Range<Tdst>(dMin, dMax)) { }
        public static ConversionRanges<Tdst, Tsrc> operator ~(ConversionRanges<Tsrc, Tdst> value) =>
            new ConversionRanges<Tdst, Tsrc>(value.Dst, value.Src);
        public ConversionRanges<double, double> ToDouble =>
            new ConversionRanges<double, double>(Src.ToDouble, Dst.ToDouble);
        public ConversionRanges<float, float> ToFloat =>
            new ConversionRanges<float, float>(Src.ToFloat, Dst.ToFloat);
    }
}
