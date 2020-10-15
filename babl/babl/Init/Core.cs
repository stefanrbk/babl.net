using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace babl.Init
{
    internal static class Core
    {
        public static void Init()
        {
            var comR = BablComponent.Create("R", Ids.Red, hasLuma: true, hasChroma: true);
            var comG = BablComponent.Create("G", Ids.Green, hasLuma: true, hasChroma: true);
            var comB = BablComponent.Create("B", Ids.Blue, hasLuma: true, hasChroma: true);
            var comA = BablComponent.Create("A", Ids.Alpha, hasAlpha: true);
            var pad = BablComponent.Create("PAD", Ids.Padding);
        }
    }
}
