using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace babl
{
    public abstract class Babl
    {
        internal const int Magic = 0xbab100;

        internal ClassType Id;
        internal string Name;
    }

    enum ClassType : int
    {
        Instance = Babl.Magic,
        Type,
        TypeInteger,
        TypeFloat,
        Sampling,
        Trc,
        Component,
        Model,
        Format,
        Space,

        Conversion,
        ConversionLinear,
        ConversionPlane,
        ConversionPlanar,

        Fish,
        FishReference,
        FishSimple,
        FishPath,
        Image,

        Extension,

        Sky
    }
}
