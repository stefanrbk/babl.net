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

        internal ClassType ClassType;
        internal int Id;
        internal string Name;
        internal string Docs;

        internal Babl(ClassType classType, string name, int id, string docs)
        {
            ClassType = classType;
            Name = name;
            Id = id;
            Docs = docs;
        }

        static Babl()
        {
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
    }

    public delegate void BablEachFunc(Babl babl);
}
