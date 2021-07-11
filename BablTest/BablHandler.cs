using NUnit.Framework;

using System;
using System.Runtime.InteropServices;

using babl;

namespace BablTest
{
    [SetUpFixture]
    public class BablHandler
    {
        const string DllPath = "libbabl-0.1-0.dll";

        public const string Info = "Info";
        public const string Parity = "Parity";
        public const string Identity = "Identity";

        [OneTimeSetUp]
        public void Init() => 
            BablInit();

        [OneTimeTearDown]
        public void Exit() =>
            BablExit();

        [DllImport(DllPath, EntryPoint = "babl_component", CharSet = CharSet.Ansi)]
        internal static extern IntPtr BablComponent(string name);

        [DllImport(DllPath, EntryPoint = "babl_component_new", CharSet = CharSet.Ansi)]
        internal static extern IntPtr BablComponentNew(__arglist);

        [DllImport(DllPath, EntryPoint = "babl_conversion_get_destination_space")]
        internal static extern IntPtr BablConversionGetDestinationSpace(IntPtr conversion);

        [DllImport(DllPath, EntryPoint = "babl_conversion_get_source_space")]
        internal static extern IntPtr BablConversionGetSourceSpace(IntPtr source);

        [DllImport(DllPath, EntryPoint = "babl_conversion_new", CharSet = CharSet.Ansi)]
        internal static extern IntPtr BablConversionNew(__arglist);

        [DllImport(DllPath, EntryPoint = "babl_cpu_accel_get_support")]
        internal static extern int BablCpuAccelGetSupport();

        [DllImport(DllPath, EntryPoint = "babl_db_each")]
        internal static extern void BablDbEach(IntPtr db, IntPtr eachFun, IntPtr userData);

        [DllImport(DllPath, EntryPoint = "babl_db_exist_by_id")]
        internal static extern IntPtr BablDbExistById(IntPtr db, int id);

        [DllImport(DllPath, EntryPoint = "babl_db_exist_by_name", CharSet = CharSet.Ansi)]
        internal static extern IntPtr BablDbExistByName(IntPtr db, string name);

        [DllImport(DllPath, EntryPoint = "babl_db_find", CharSet = CharSet.Ansi)]
        internal static extern IntPtr BablDbFind(IntPtr db, string name);

        [DllImport(DllPath, EntryPoint = "babl_db_init")]
        internal static extern IntPtr BablDbInit();

        [DllImport(DllPath, EntryPoint = "babl_exit")]
        internal static extern void BablExit();

        [DllImport(DllPath, EntryPoint = "babl_extender")]
        internal static extern IntPtr BablExtender();

        [DllImport(DllPath, EntryPoint = "babl_extension_quiet_log")]
        internal static extern IntPtr BablExtensionQuietLog();

        [DllImport(DllPath, EntryPoint = "babl_fast_fish", CharSet = CharSet.Ansi)]
        internal static extern void BablFastFish(IntPtr sourceFormat, IntPtr destinationFormat, string performance);

        [DllImport(DllPath, EntryPoint = "babl_fish")]
        internal static extern void BablFish(IntPtr sourceFormat, IntPtr destinationFormat);

        [DllImport(DllPath, EntryPoint = "babl_fish_db")]
        internal static extern IntPtr BablFishDb();

        [DllImport(DllPath, EntryPoint = "babl_fish_path")]
        internal static extern void BablFishPath(IntPtr sourceFormat, IntPtr destinationFormat);

        [DllImport(DllPath, EntryPoint = "babl_format", CharSet = CharSet.Ansi)]
        internal static extern IntPtr BablFormat(string name);

        [DllImport(DllPath, EntryPoint = "babl_format_exists", CharSet = CharSet.Ansi)]
        internal static extern bool BablFormatExists(string name);

        [DllImport(DllPath, EntryPoint = "babl_format_get_bytes_per_pixel")]
        internal static extern int BablFormatGetBytesPerPixel(IntPtr format);

        [DllImport(DllPath, EntryPoint = "babl_format_get_encoding")]
        internal static extern string? BablFormatGetEncoding(IntPtr format);

        [DllImport(DllPath, EntryPoint = "babl_format_get_model")]
        internal static extern IntPtr BablFormatGetModel(IntPtr format);

        [DllImport(DllPath, EntryPoint = "babl_format_get_n_components")]
        internal static extern int BablFormatGetNComponents(IntPtr format);

        [DllImport(DllPath, EntryPoint = "babl_format_get_space")]
        internal static extern IntPtr BablFormatGetSpace(IntPtr format);

        [DllImport(DllPath, EntryPoint = "babl_format_get_type")]
        internal static extern IntPtr BablFormatGetType(IntPtr format, int componentIndex);

        [DllImport(DllPath, EntryPoint = "babl_format_has_alpha")]
        internal static extern bool BablFormatHasAlpha(IntPtr format);

        [DllImport(DllPath, EntryPoint = "babl_format_is_format_n")]
        internal static extern bool BablFormatIsFormatN(IntPtr format);

        [DllImport(DllPath, EntryPoint = "babl_format_is_palette")]
        internal static extern bool BablFormatIsPalette(IntPtr format);

        [DllImport(DllPath, EntryPoint = "babl_format_n")]
        internal static extern IntPtr BablFormatN(IntPtr type, int components);

        [DllImport(DllPath, EntryPoint = "babl_format_new")]
        internal static extern IntPtr BablFormatNew(__arglist);

        [DllImport(DllPath, EntryPoint = "babl_format_with_space")]
        internal static extern IntPtr BablFormatWithSpace(IntPtr encoding, IntPtr space);

        [DllImport(DllPath, EntryPoint = "babl_formats_count")]
        internal static extern int BablFormatsCount();

        [DllImport(DllPath, EntryPoint = "babl_free")]
        internal static extern void BablFree(IntPtr ptr);

        [DllImport(DllPath, EntryPoint = "babl_get_model_flags")]
        internal static extern BablModelFlag BablGetModelFlags(IntPtr babl);

        [DllImport(DllPath, EntryPoint = "babl_get_name", CharSet = CharSet.Ansi)]
        internal static extern string BablGetName(IntPtr babl);

        [DllImport(DllPath, EntryPoint = "babl_get_user_data")]
        internal static extern IntPtr BablGetUserData(IntPtr babl);

        [DllImport(DllPath, EntryPoint = "babl_get_version")]
        internal static extern void BablGetVersion(IntPtr major, IntPtr minor, IntPtr micro);

        [DllImport(DllPath, EntryPoint = "babl_icc_get_key", CharSet = CharSet.Ansi)]
        internal static extern IntPtr BablIccGetKey(IntPtr iccData, int iccLength, string key, IntPtr language, IntPtr country);

        [DllImport(DllPath, EntryPoint = "babl_icc_make_space")]
        internal static extern IntPtr BablIccMakeSpace(IntPtr iccData, int iccLength, BablIccIntent intent, IntPtr error);

        [DllImport(DllPath, EntryPoint = "babl_init")]
        internal static extern void BablInit();

        [DllImport(DllPath, EntryPoint = "babl_introspect")]
        internal static extern void BablIntrospect(IntPtr babl);

        [DllImport(DllPath, EntryPoint = "babl_malloc")]
        internal static extern IntPtr BablAllocate(nint size);

        [DllImport(DllPath, EntryPoint = "babl_model", CharSet = CharSet.Ansi)]
        internal static extern IntPtr BablModel(string name);

        [DllImport(DllPath, EntryPoint = "babl_model_is", CharSet = CharSet.Ansi)]
        internal static extern bool BablModelIs(IntPtr babl, string modelName);

        [DllImport(DllPath, EntryPoint = "babl_model_is_symmetric")]
        internal static extern double BablModelIsSymmetric(IntPtr babl);

        [DllImport(DllPath, EntryPoint = "babl_model_new")]
        internal static extern IntPtr BablModelNew(__arglist);

        [DllImport(DllPath, EntryPoint = "babl_model_with_space", CharSet = CharSet.Ansi)]
        internal static extern IntPtr BablModelWithSpace(string name, IntPtr space);

        [DllImport(DllPath, EntryPoint = "babl_new_palette", CharSet = CharSet.Ansi)]
        internal static extern IntPtr BablNewPalette(string name, IntPtr formatU8, IntPtr formatU8WithAlpha);

        [DllImport(DllPath, EntryPoint = "babl_new_palette_with_space", CharSet = CharSet.Ansi)]
        internal static extern IntPtr BablNewPaletteWithSpace(string name, IntPtr space, IntPtr formatU8, IntPtr formatU8WithAlpha);

        [DllImport(DllPath, EntryPoint = "babl_palette_reset")]
        internal static extern void BablPaletteReset(IntPtr babl);

        [DllImport(DllPath, EntryPoint = "babl_palette_set_palette")]
        internal static extern void BablPaletteSetPalette(IntPtr babl, IntPtr format, IntPtr data, int count);

        [DllImport(DllPath, EntryPoint = "babl_polynomial_approximate_gamma")]
        internal static extern void BablPolynomialApproximateGamma(IntPtr poly, double gamma, double x0, double x1, int degree, int scale);

        [DllImport(DllPath, EntryPoint = "babl_process")]
        internal static extern long BablProcess(IntPtr babl, IntPtr source, IntPtr destination, long n);

        [DllImport(DllPath, EntryPoint = "babl_process_rows")]
        internal static extern long BablProcessRows(IntPtr babl, IntPtr source, int sourceStride, IntPtr destination, int destStride, long n, int rows);

        [DllImport(DllPath, EntryPoint = "babl_sampling")]
        internal static extern IntPtr BablSampling(int horizontal, int vertical);

        [DllImport(DllPath, EntryPoint = "babl_sanity")]
        internal static extern bool BablSanity();

        [DllImport(DllPath, EntryPoint = "babl_set_extender")]
        internal static extern void BablSetExtender(IntPtr newExtender);

        [DllImport(DllPath, EntryPoint = "babl_set_user_data")]
        internal static extern void BablSetUserData(IntPtr babl, IntPtr data);

        [DllImport(DllPath, EntryPoint = "babl_space", CharSet = CharSet.Ansi)]
        internal static extern IntPtr BablSpace(string name);

        [DllImport(DllPath, EntryPoint = "babl_space_from_chromaticities", CharSet = CharSet.Ansi)]
        internal static extern IntPtr BablSpaceFromChromaticities(string name, double wx, double wy, double rx, double ry, double gx, double gy, double bx, double by, IntPtr trcRed, IntPtr trcGreen, IntPtr trcBlue, BablSpaceFlag flags);

        [DllImport(DllPath, EntryPoint = "babl_space_from_icc")]
        internal static extern IntPtr BablSpaceFromIcc(IntPtr iccData, int iccLength, BablIccIntent intent, IntPtr error);

        [DllImport(DllPath, EntryPoint = "babl_space_from_rgbxyz_matrix", CharSet = CharSet.Ansi)]
        internal static extern IntPtr BablSpaceFromRgbxyzMatrix(string name, double wx, double wy, double wz, double rx, double gx, double bx, double ry, double gy, double by, double rz, double gz, double bz, IntPtr trcRed, IntPtr trcGreen, IntPtr trcBlue);

        [DllImport(DllPath, EntryPoint = "babl_space_from_xyz")]
        internal static extern void BablSpaceFromXyz(IntPtr space, IntPtr xyz, IntPtr rgb);

        [DllImport(DllPath, EntryPoint = "babl_space_get")]
        internal static extern void BablSpaceGet(IntPtr space, IntPtr xw, IntPtr yw, IntPtr xr, IntPtr yr, IntPtr xg, IntPtr yg, IntPtr xb, IntPtr yb, IntPtr redTrc, IntPtr greenTrc, IntPtr blueTrc);

        [DllImport(DllPath, EntryPoint = "babl_space_get_icc")]
        internal static extern IntPtr BablSpaceGetIcc(IntPtr babl, IntPtr length);

        [DllImport(DllPath, EntryPoint = "babl_space_get_rgb_luminance")]
        internal static extern void BablSpaceGetRgbLuminance(IntPtr babl, IntPtr redLuminance, IntPtr greenLuminance, IntPtr blueLuminance);

        [DllImport(DllPath, EntryPoint = "babl_space_get_rgbtoxyz")]
        internal static extern IntPtr BablSpaceGetRgbtoxyz(IntPtr babl);

        [DllImport(DllPath, EntryPoint = "babl_space_is_cmyk")]
        internal static extern bool BablSpaceIsCmyk(IntPtr space);

        [DllImport(DllPath, EntryPoint = "babl_space_is_gray")]
        internal static extern bool BablSpaceIsGray(IntPtr space);

        [DllImport(DllPath, EntryPoint = "babl_space_to_icc")]
        internal static extern IntPtr BablSpaceToIcc(IntPtr space, IntPtr description, IntPtr copyright, BablIccFlag flags, IntPtr iccLength);

        [DllImport(DllPath, EntryPoint = "babl_space_to_xyz")]
        internal static extern void BablSpaceToXyz(IntPtr space, IntPtr rgb, IntPtr xyz);

        [DllImport(DllPath, EntryPoint = "babl_space_with_trc")]
        internal static extern IntPtr BablSpaceWithTrc(IntPtr space, IntPtr trc);

        [DllImport(DllPath, EntryPoint = "babl_ticks")]
        internal static extern long BablTicks();

        [DllImport(DllPath, EntryPoint = "babl_trc", CharSet = CharSet.Ansi)]
        internal static extern IntPtr BablTrc(string name);

        [DllImport(DllPath, EntryPoint = "babl_trc_gamma")]
        internal static extern IntPtr BablTrcGamma(double gamma);

        [DllImport(DllPath, EntryPoint = "babl_type", CharSet = CharSet.Ansi)]
        internal static extern IntPtr BablType(string name);

        [DllImport(DllPath, EntryPoint = "babl_type_is_symmetric")]
        internal static extern bool BablTypeIsSymmetric(IntPtr babl);

        [DllImport(DllPath, EntryPoint = "babl_type_new")]
        internal static extern IntPtr BablTypeNew(__arglist);


        public struct Instance
        {
            public int ClassType;
            public int Id;
            public IntPtr Creator;
            public IntPtr Name;
            public IntPtr Docs;
        }
        public struct Db
        {
            public IntPtr names;
            public IntPtr ids;
            public IntPtr babls;
            public IntPtr mutex;
        }
    }
}
