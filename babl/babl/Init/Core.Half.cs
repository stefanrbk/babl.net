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
        public static void TypeHalfInit()
        {
            CreateType("half", id: Ids.Half, bits: 16, doc: "IEEE 754 half precision.");
        }
    }
}
