
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

        internal int Id;
        internal string Name;
        internal string Docs;

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
