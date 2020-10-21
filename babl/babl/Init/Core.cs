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
            InitTypes();

            _ = BablComponent.Create("R", Ids.Red, hasLuma: true, hasChroma: true);
            _ = BablComponent.Create("G", Ids.Green, hasLuma: true, hasChroma: true);
            _ = BablComponent.Create("B", Ids.Blue, hasLuma: true, hasChroma: true);
            _ = BablComponent.Create("A", Ids.Alpha, hasAlpha: true);
            _ = BablComponent.Create("PAD", Ids.Padding);
        }

        private static void InitTypes()
        {
            TypeFloatInit();
            TypeHalfInit();
            TypeU8Init();
        }
    }
}
