using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace babl
{
    internal class StringComparer : IEqualityComparer<string>
    {
        public bool Equals([AllowNull] string x, [AllowNull] string y) =>
            GetHashCode(x ?? "") == GetHashCode(y ?? "");

        public int GetHashCode([DisallowNull] string obj)
        {
            var hash = 0;
            foreach (var c in obj)
                hash = HashCode.Combine(hash, c);
            return hash;
        }
    }
}
