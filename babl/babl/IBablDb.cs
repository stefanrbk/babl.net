using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace babl
{
    internal interface IBablDb
    {
        public static readonly BablDb db = new BablDb();
    }
}
