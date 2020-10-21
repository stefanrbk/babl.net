using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace babl.Init
{
    internal static partial class Core
    {
        public static void Init()
        {
            Babl.logOnNameLookups = true;

            InitTypes();

            Babl.logOnNameLookups = false;
        }

        private static void InitTypes()
        {
            TypeDoubleInit();
            TypeFloatInit();
            TypeU15Init();
            TypeHalfInit();
            TypeU8Init();
            TypeU16Init();
            TypeU32Init();
        }
    }
}
