﻿using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace babl
{
    public abstract partial class Babl
    {
        internal static bool logOnNameLookups = false;

        internal static Babl? BABL(object? obj) =>
            obj as Babl;
        internal static void Log(string message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0) =>
            Console.Write($"{filePath}:{lineNumber} {memberName}()\n\t{message}\n");

        [DoesNotReturn]
        internal static void Error(string message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            Console.Error.Write($"{filePath}:{lineNumber} {memberName}()\n\t{message}\n");
            Environment.Exit(-1);
        }

        internal static void Assert([DoesNotReturnIf(false)] bool value, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            if (!value)
            {
                Error("Assertion failed", memberName, filePath, lineNumber);
                Environment.Exit(-1);
            }
        }
    }
}