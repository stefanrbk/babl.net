using System;
using System.Collections;
using System.Collections.Generic;

namespace babl
{
    public abstract partial class Babl : IEnumerable<Babl>
    {
        internal string Name { get; set; } = "";
        internal abstract BablClassType ClassType { get; }
        internal int Id { get; set; }
        internal string Doc { get; set; } = "";

        public abstract IEnumerator<Babl> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => 
            GetEnumerator();
    }
}
