using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace babl
{
    internal static class Logging
    {
        public static void LookingUp(string name,
                  [CallerMemberName] string memberName = "",
                    [CallerFilePath] string filePath = "",
                  [CallerLineNumber] int lineNumber = 0) =>
            Babl.Log($"\"{name}\": looking up", memberName, filePath, lineNumber);
    }
}
