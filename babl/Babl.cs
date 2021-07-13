
using NLog;
using NLog.Config;
using NLog.Targets;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace babl
{
    public abstract class Babl
    {
        internal const int Magic = 0xbab100;
#if DEBUG
        public int Id;
        public string Name;
        public string Docs;
        public object? Creator;
#else
        internal int Id;
        internal string Name;
        internal string Docs;
        internal object? Creator;
#endif
        internal Babl(string name, int id, string docs)
        {
            Name = name;
            Id = id;
            Docs = docs;
        }

        [ModuleInitializer]
        public static void ModuleCtor()
        {
            var config = new LoggingConfiguration();
            var logconsole = new ColoredConsoleTarget();
            config.AddRuleForAllLevels(logconsole);
            LogManager.Configuration = config;

            BablComponent.InitBase();
            BablType.InitBase();
        }

        public static Babl Component(string name) =>
            BablComponent.Find(name) ?? throw new ArgumentException("BablComponent does not exist", nameof(name));
        public static Babl ComponentNew(string name = "", int id = 0, bool luma = false, bool chroma = false, bool alpha = false, string docs = "") =>
            BablComponent.New(name, id, luma, chroma, alpha, docs);
        public static Babl ComponentNew(BablId id, string name = "", bool luma = false, bool chroma = false, bool alpha = false, string docs = "") =>
            BablComponent.New(id, name, luma, chroma, alpha, docs);
        public static void ConversionClassForEach(BablEachFunc eachFunc)
        {
            throw new NotImplementedException();
        }
        public static void ConversionGetDestinationSpace()
        {
            throw new NotImplementedException();
        }
        public static void ConversionGetSourceSpace()
        {
            throw new NotImplementedException();
        }
        public static void ConversionNew()
        {
            throw new NotImplementedException();
        }
        public static void CpuAccelGetSupport()
        {
            throw new NotImplementedException();
        }
        public static void DbEach()
        {
            throw new NotImplementedException();
        }
        public static void DbExistById()
        {
            throw new NotImplementedException();
        }
        public static void DbFind()
        {
            throw new NotImplementedException();
        }
        public static void DbInit()
        {
            throw new NotImplementedException();
        }
        public static void Exit()
        {
            throw new NotImplementedException();
        }
        public static Babl Extender()
        {
            throw new NotImplementedException();
        }
        public static Babl ExtensionQuietLog()
        {
            throw new NotImplementedException();
        }
        public static void FastFish()
        {
            throw new NotImplementedException();
        }
        public static void Fish(string name)
        {
            throw new NotImplementedException();
        }
        public static BablDb FishDb()
        {
            throw new NotImplementedException();
        }
        public static Babl FishPath(Babl source, Babl destination)
        {
            throw new NotImplementedException();
        }
        public static void Format(string name)
        {
            throw new NotImplementedException();
        }
        public static void FormatClassForEach(BablEachFunc eachFunc)
        {
            throw new NotImplementedException();
        }
        public static void FormatExists()
        {
            throw new NotImplementedException();
        }
        public static void FormatGetBytesPerPixel()
        {
            throw new NotImplementedException();
        }
        public static void FormatGetEncoding()
        {
            throw new NotImplementedException();
        }
        public static void FormatGetModel()
        {
            throw new NotImplementedException();
        }
        public static void FormatGetNComponents()
        {
            throw new NotImplementedException();
        }
        public static void FormatGetSpace()
        {
            throw new NotImplementedException();
        }
        public static void FormatGetType()
        {
            throw new NotImplementedException();
        }
        public static void FormatHasAlpha()
        {
            throw new NotImplementedException();
        }
        public static void FormatIsFormatN()
        {
            throw new NotImplementedException();
        }
        public static void FormatIsPallette()
        {
            throw new NotImplementedException();
        }
        public static void FormatN()
        {
            throw new NotImplementedException();
        }
        public static void FormatNew()
        {
            throw new NotImplementedException();
        }
        public static void FormatWithSpace()
        {
            throw new NotImplementedException();
        }
        public static int FormatsCount()
        {
            throw new NotImplementedException();
        }
        public static void GetModelFlags()
        {
            throw new NotImplementedException();
        }
        public static void GetName()
        {
            throw new NotImplementedException();
        }
        public static void GetUserData()
        {
            throw new NotImplementedException();
        }
        public static void GetVersion()
        {
            throw new NotImplementedException();
        }
        public static void IccGetKey()
        {
            throw new NotImplementedException();
        }
        public static void IccMakeSpace()
        {
            throw new NotImplementedException();
        }
        public static void Init()
        {
            throw new NotImplementedException();
        }
        public static void Introspect()
        {
            throw new NotImplementedException();
        }
        public static void Model(string name)
        {
            throw new NotImplementedException();
        }
        public static void ModelClassForEach(BablEachFunc eachFunc)
        {
            throw new NotImplementedException();
        }
        public static void ModelIs()
        {
            throw new NotImplementedException();
        }
        public static double ModelIsSymmetric(in Babl babl)
        {
            throw new NotImplementedException();
        }
        public static void ModelNew()
        {
            throw new NotImplementedException();
        }
        public static void ModelWithSpace()
        {
            throw new NotImplementedException();
        }
        public static void PaletteReset()
        {
            throw new NotImplementedException();
        }
        public static void PaletteSetPalette()
        {
            throw new NotImplementedException();
        }
        public static void PolynomialApproximateGamma()
        {
            throw new NotImplementedException();
        }
        public static void Process()
        {
            throw new NotImplementedException();
        }
        public static void ProcessRows()
        {
            throw new NotImplementedException();
        }
        public static void Sampling(string name)
        {
            throw new NotImplementedException();
        }
        public static int Sanity()
        {
            throw new NotImplementedException();
        }
        public static void SetExtender(Babl newExtender)
        {
            throw new NotImplementedException();
        }
        public static void SetUserData()
        {
            throw new NotImplementedException();
        }
        public static void Space(string name)
        {
            throw new NotImplementedException();
        }
        public static void SpaceFromChromaticities()
        {
            throw new NotImplementedException();
        }
        public static void SpaceFromIcc()
        {
            throw new NotImplementedException();
        }
        public static void SpaceFromRgbxyzMatrix()
        {
            throw new NotImplementedException();
        }
        public static void SpaceFromXyz(in Babl space, in double[] xyz, ref double[] rgb)
        {
            throw new NotImplementedException();
        }
        public static void SpaceGet()
        {
            throw new NotImplementedException();
        }
        public static void SpaceGetIcc()
        {
            throw new NotImplementedException();
        }
        public static void SpaceGetRgbLuminance()
        {
            throw new NotImplementedException();
        }
        public static double[] SpaceGetRgbToXyz(in Babl space)
        {
            throw new NotImplementedException();
        }
        public static void SpaceIsCmyk()
        {
            throw new NotImplementedException();
        }
        public static void SpaceIsGray()
        {
            throw new NotImplementedException();
        }
        public static IntPtr SpaceToIcc(in Babl space, string description, string copyright, int flags, out int iccLength)
        {
            throw new NotImplementedException();
        }
        public static void SpaceToXyz(in Babl space, in double[] rgb, ref double[] xyz)
        {
            throw new NotImplementedException();
        }
        public static void SpaceWithTrc()
        {
            throw new NotImplementedException();
        }
        public static void Ticks()
        {
            throw new NotImplementedException();
        }
        public static void Trc(string name)
        {
            throw new NotImplementedException();
        }
        public static void TrcGamma()
        {
            throw new NotImplementedException();
        }
        public static Babl Type(string name) =>
            BablType.Find(name) ?? throw new ArgumentException("BablType does not exist", nameof(name));
        public static void TypeClassForEach(BablEachFunc eachFunc)
        {
            throw new NotImplementedException();
        }
        public static int TypeIsSymmetric(in Babl babl)
        {
            throw new NotImplementedException();
        }
        public static Babl TypeNew(string name = "", int id = 0, int bits = 0, string docs = "") =>
            BablType.New(name, id, bits, docs);
        public static Babl TypeNew(BablId id, string name = "", int bits = 0, string docs = "") =>
            BablType.New(name, id, bits, docs);

        internal static void Assert(bool value, 
                                    string message = "", 
                   [CallerFilePath] string path = "", 
                 [CallerLineNumber] int line = 0, 
                 [CallerMemberName] string method = "")
        {
            if (!value)
                if (message == "")
                    throw new Exception($"Eeeeek! Assertion failed at line: {line} in {method} within {path}.");
                else
                    throw new Exception($"Eeeeek! Assertion failed: {message} at line {line} in {method} within {path}.");

        }

        internal static void Log(string message) =>
            LogManager.GetCurrentClassLogger().Warn(message);

        internal static void Fatal(string message) =>
            LogManager.GetCurrentClassLogger().Fatal(message);

    }

    public delegate void BablEachFunc(Babl babl);
}
