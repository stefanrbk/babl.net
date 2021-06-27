using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace babl
{
    public abstract class Babl
    {
        internal const int Magic = 0xbab100;

        internal int Id;
        internal string Name;
        internal string Docs;

        internal Babl(string name, int id, string docs)
        {
            Name = name;
            Id = id;
            Docs = docs;
        }

        static Babl()
        {
            BablComponent.InitBase();
            BablType.InitBase();
        }
    }

    public delegate void BablEachFunc(Babl babl);
}
