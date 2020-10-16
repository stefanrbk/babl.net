using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace babl
{
    internal static class Fatal
    {
        [DoesNotReturn]
        public static void AlreadyRegistered(string name,
                                             string className,
                          [CallerMemberName] string memberName = "",
                            [CallerFilePath] string filePath = "",
                          [CallerLineNumber] int lineNumber = 0) =>
            Babl.Error($"Trying to reregister {className} '{name}' with different id!", memberName, filePath, lineNumber);
        [DoesNotReturn]
        public static void ExistsAsDifferentValue(string name,
                                                  string className,
                               [CallerMemberName] string memberName = "",
                                 [CallerFilePath] string filePath = "",
                               [CallerLineNumber] int lineNumber = 0) =>
            Babl.Error($"{className} '{name}' already registered with different attributes!", memberName, filePath, lineNumber);
        [DoesNotReturn]
        public static void NotFound(string name,
                 [CallerMemberName] string memberName = "",
                   [CallerFilePath] string filePath = "",
                 [CallerLineNumber] int lineNumber = 0) =>
            Babl.Error($"\"{name}\": not found");
    }
}
