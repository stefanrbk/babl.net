using System;
using System.Linq;
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

        internal Babl(ClassType classType, string name, int id = 0)
        {
            ClassType = classType;
            Name = name;
            Id = id;
        }
    }
}
