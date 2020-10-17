using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace babl
{
    public abstract partial class Babl
    {
        public static void Init()
        {
            lock (refCountLock)
            {
                if (refCount++ == 0)
                {
                    // Inits go here
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

        //public static Babl? Sampling(int horizontal, int vertical)
        //{

        //}

        public static Babl Component(string name) =>
            BablComponent.Find(name);

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

        //public static string GetName(Babl babl)
        //{

        //}

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

        public static Babl CreateType(string name, int id = 0, int bits = 0, string doc = "") =>
            BablType.Create(name, id, bits, doc);

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

        //public static Babl? CreateConversion(params object[] args)
        //{

        //}

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

        //public static Babl? TrcGamma(double gamma)
        //{

        //}

        //public static Babl? Trc(string name)
        //{

        //}

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
