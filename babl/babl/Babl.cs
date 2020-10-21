using System;
using System.Collections;
using System.Collections.Generic;

namespace babl
{
    public abstract partial class Babl
    {
        internal string Name { get; set; } = "";
        internal abstract BablClassType ClassType { get; }
        internal int Id { get; set; }
        internal string Doc { get; set; } = "";

        public override string ToString() =>
            $"Name: \"{Name}\"\n" +
            $"Type: {ClassType}\n" +
            $"Id: {Id}" + (!string.IsNullOrEmpty(Doc) ? $"\nDoc: {Doc}" : "");
    }
}
