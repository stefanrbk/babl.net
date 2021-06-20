using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace babl
{
    public abstract class Babl
    {
        internal const int Magic = 0xbab100;

        internal ClassType Id;
        internal string Name;
    }
}
