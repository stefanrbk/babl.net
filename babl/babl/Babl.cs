using System;

namespace babl
{
    public abstract partial class Babl
    {
        private static volatile int refCount;
        private static readonly object refCountLock = new object();

        internal const int Magic = 0xbab100;

        internal string Name { get; set; } = "";
        internal BablClassType ClassType { get; set; }
        internal int Id { get; set; }
        internal string Doc { get; set; } = "";
    }
}
