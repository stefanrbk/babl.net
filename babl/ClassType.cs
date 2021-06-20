namespace babl
{
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
