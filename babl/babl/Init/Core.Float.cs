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
        public static void TypeFloatInit()
        {
            CreateType("float", id: Float, bits: 32, doc: "IEEE 754 single precision");
        }
    }
}
