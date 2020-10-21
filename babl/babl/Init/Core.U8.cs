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
        private static void TypeU8Init()
        {
            CreateType("u8", id: U8, bits: 8, doc: "byte, 8 bit unsigned integer, values from 0-255");
            CreateType("u8-luma", id: U8Luma, bits: 8, doc: "8 bit unsigned integer, valuers from 16-235");
            CreateType("u8-chroma", id: U8Chroma, integer: true, unsigned: true, bits: 8, min: 16L, max: 240L, minVal: -0.5, maxVal: 0.5, doc: "8 bit unsigned integer -0.5 to 0.5 maps to 16-240");
        }
    }
}
