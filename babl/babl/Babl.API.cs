using babl.Init;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace babl
{
    public abstract partial class Babl
    {
        private static volatile int refCount;
        private static readonly object refCountLock = new object();

        public delegate void FuncDispatch(Babl babl, object src, object dst, long num, object userData);
        public delegate void FuncPlane(Babl conversion, object src, object dst, int srcPitch, int dstPitch, long num, object userData);
        public delegate void FuncLinear(Babl conversion, object src, object dst, long num, object userData);
        public delegate void FuncPlanar(Babl conversion, int srcBands, object[] src, int[] srcPitch, int dstBands, object[] dst, int[] dstPitch, long num, object userData);

        public static void Init()
        {
            lock (refCountLock)
            {
                if (refCount++ == 0)
                {
                    //BablTrc.Init();
                    Core.Init();
                }
            }
        }

        public static void Exit()
        {
            lock (refCountLock)
            {
                if (--refCount == 0)
                {
                    // Deinits go here
                }
            }
        }

        public static Babl Type(string name) =>
            BablType.Find(name);
        public static Babl Type(int id) =>
            BablType.Find(id);
        public static void TypeForEach(Action<Babl> action) =>
            BablType.ForEach(action);

        //public static Babl? Sampling(int horizontal, int vertical)
        //{

        //}

        public static Babl Component(string name) =>
            BablComponent.Find(name);
        public static Babl Component(int id) =>
            BablComponent.Find(id);
        public static void ComponentForEach(Action<Babl> action) =>
            BablComponent.ForEach(action);

        //public static Babl? Model(string name)
        //{

        //}

        //public static Babl? ModelWithSpace(string name, Babl space)
        //{

        //}

        //public static Babl? Space(string name)
        //{

        //}

        //public static Babl? SpaceFromIcc(ReadOnlySpan<byte> iccData, BablIccIntent intent, out string error)
        //{

        //}

        //public static double SpaceGetGamma(Babl space)
        //{

        //}

        //public static string? IccGetKey(ReadOnlySpan<byte> iccData, string key, string language, string country)
        //{

        //}

        //public static Babl? Format(string encoding)
        //{

        //}

        //public static Babl? FormatWithSpace(string encoding, in Babl space)
        //{

        //}

        //public static bool FormatExists(string name)
        //{

        //}

        //public static Babl? FormatGetSpace(Babl format)
        //{

        //}

        //public static Babl? Fish(Babl sourceFormat, Babl destinationFormat)
        //{

        //}
        //public static Babl? Fish(Babl sourceFormat, string destinationFormat)
        //{

        //}
        //public static Babl? Fish(string sourceFormat, Babl destinationFormat)
        //{

        //}
        //public static Babl? Fish(string sourceFormat, string destinationFormat)
        //{

        //}

        //public static Babl? FastFish(Babl sourceFormat, Babl destinationFormat, BablFishPerformance performance)
        //{

        //}
        //public static Babl? FastFish(Babl sourceFormat, string destinationFormat, BablFishPerformance performance)
        //{

        //}
        //public static Babl? FastFish(string sourceFormat, Babl destinationFormat, BablFishPerformance performance)
        //{

        //}
        //public static Babl? FastFish(string sourceFormat, string destinationFormat, BablFishPerformance performance)
        //{

        //}

        //public static long Process(Babl fish, ReadOnlySpan<byte> source, Span<byte> destination, long pixelsToProcess)
        //{

        //}

        //public static long ProcessRows(Babl fish, ReadOnlySpan<byte> source, int sourceStride, Span<byte> destination, int destinationStride, long pixelsToProcess, int rows)
        //{

        //}

        public static string GetName(Babl babl) =>
            babl.Name;

        //public static bool FormatHasAlpha(Babl format)
        //{

        //}

        //public static int FormatGetBytesPerPixel(Babl format)
        //{

        //}

        //public static Babl? FormatGetModel(Babl format)
        //{

        //}

        //public static BablModelFlags ModelGetFlags(Babl model)
        //{

        //}

        //public static int FormatGetComponentCount(Babl format)
        //{

        //}

        //public static Babl? FormatGetType(Babl format, int componentIndex)
        //{

        //}

        public static Babl CreateType(string name, int id = 0, int bits = 0, bool integer = false, bool unsigned = false, long min = 0, long max = 0, double minVal = 0, double maxVal = 0, string doc = "") =>
            BablType.Create(name, id, bits, integer, unsigned, min, max, minVal, maxVal, doc);

        public static Babl CreateComponent(string name, int id, bool hasLuma = false, bool hasChroma = false, bool hasAlpha = false, string doc = "") =>
            BablComponent.Create(name, id, hasLuma, hasChroma, hasAlpha, doc);

        //public static Babl? CreateModel(params object[] args)
        //{

        //}

        //public static Babl? CreateFormat(params object[] args)
        //{

        //}

        //public static Babl? Format2(Babl type, int components)
        //{

        //}

        //public static bool FormatIsFormat2(Babl format)
        //{

        //}

        public static Babl? CreateConversion(Babl source,
                                             Babl destination,
                                             int id = 0,
                                             object? data = null,
                                             bool allowCollisions = false,
                                             FuncLinear? linear = null,
                                             FuncPlane? plane = null,
                                             FuncPlanar? planar = null) =>
            BablConversion.Create(source, destination, id, data, allowCollisions, linear, plane, planar);

        //public static Babl? ConversionGetSourceSpace(Babl conversion)
        //{

        //}

        //public static Babl? ConversionGetDestinationSpace(Babl conversion)
        //{

        //}

        //public static Babl? CreatePalette(string name, Babl[]? format = null, Babl[]? formatWithAlpha = null)
        //{

        //}

        //public static Babl? CreatePaletteWithSpace(string name, Babl space, Babl[]? format = null, Babl[]? formatWithAlpha = null)
        //{

        //}

        //public static bool FormatIsPalette(Babl format)
        //{

        //}

        //public static void PaletteSetPalette(Babl babl, Babl format, ReadOnlySpan<byte> data, int count)
        //{

        //}

        //public static void PaletteReset(Babl babl)
        //{

        //}

        //public static void SetUserData(Babl babl, object data)
        //{

        //}

        //public static object GetUserData(Babl babl)
        //{

        //}

        //public static Babl? SpaceFromChromaticities(string name, double wx, double wy, double rx, double ry, double gx,
        //                                            double gy, double bx, double by, Babl trcRed, Babl? trcGreen,
        //                                            Babl? trcBlue, BablSpaceFlags flags)
        //{

        //}

        public static Babl? TrcGamma(double gamma) =>
            BablTrc.FormulaGamma(gamma);

        public static Babl? Trc(string name) =>
            BablTrc.Find(name);

        public static void TrcForEach(Action<Babl> action) =>
            BablTrc.ForEach(action);

        //public static Babl? SpaceWithTrc(Babl space, Babl trc)
        //{

        //}

        //public static (double xw, double yw, double xr, double yr, double xg, double yg, double xb, double yb, Babl red, Babl green, Babl blue) GetSpace(Babl space)
        //{

        //}

        //public static (double red, double green, double blue) SpaceGetRGBLuminance(Babl space)
        //{

        //}

        //public static bool ModelIs(Babl model, string name)
        //{

        //}

        //public static ReadOnlySpan<byte> SpaceGetIcc(Babl babl)
        //{

        //}

        //public static Babl? SpaceFromRgbxyzMatrix(string? name, double wx, double wy, double wz, double rx, double gx, double bx, double ry, double gy, double by, double rz, double gz, double bz, Babl trcRed, Babl? trcGreen, Babl? trcBlue)
        //{

        //}

        //public static string FormatGetEncoding(Babl babl)
        //{

        //}

        //public static bool SpaceIsCmyk(Babl space)
        //{

        //}

        //public static bool SpaceIsGray(Babl space)
        //{

        //}
    }
}
