using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace babl
{
    public static class Ids
    {
        public const int Undefined = 0;

        public const int TypeBase = 100;
        public const int U8 = 101;
        public const int U16 = 102;

        public const int Float = 105;

        public const int U8Luma = 108;
        public const int U8Chroma = 109;

        public const int ComponentBase = 10_000;
        public const int GrayLinear = 10_001;
        public const int GrayLinearMulAlpha = 10_002;
        public const int Red = 10_003;
        public const int Green = 10_004;
        public const int Blue = 10_005;
        public const int Alpha = 10_006;

        public const int Padding = 10_034;
    }
}
