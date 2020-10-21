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
        private static void TypeU15Init()
        {
            logOnNameLookups = false;

            CreateType("u15", bits: 16);

            logOnNameLookups = true;
        }
    }
}
