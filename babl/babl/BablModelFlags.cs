using System;

namespace babl
{
    [Flags]
    public enum BablModelFlags
    {
        Alpha               = 1<<1,
        Associated          = 1<<2,
        Inverted            = 1<<3,

        Linear              = 1<<10,
        Nonlinear           = 1<<11,
        Perceptual          = 1<<12,

        Gray                = 1<<20,
        RGB                 = 1<<21,
        Spectral            = 1<<22,
        CIE                 = 1<<23,
        CMYK                = 1<<24,
        LUZ                 = 1<<25,
    }
}
