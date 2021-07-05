
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
        public
#else
        internal
#endif
        int Id;
#if DEBUG
        public
#else
        internal
# endif
        string Name;
#if DEBUG
        public
#else
        internal
#endif
        string Docs;

        internal Babl(string name, int id, string docs)
        {
            Name = name;
            Id = id;
            Docs = docs;
        }

        [ModuleInitializer]
        public static void BablInit()
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
