using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static babl.Babl;
using static babl.Ids;
namespace babl.Init
{
    internal static partial class Core
    {
        private static void TypeU16Init()
        {
            CreateType("u16", id: U16, bits: 16);
        }
    }
}
