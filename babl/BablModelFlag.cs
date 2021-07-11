using System;

namespace babl
{
    [Flags]
    public enum BablModelFlag : int
    {
        /// <summary>
        /// The model encodes alpha.
        /// </summary>
        Alpha = 1<<1,
        /// <summary>
        /// The alpha is associated alpha.
        /// </summary>
        Associated = 1<<2,
        /// <summary>
        /// The components are inverted (used for getting the additive complement space of CMYK.
        /// </summary>
        Inverted = 1 << 3,

        /// <summary>
        /// The data has no TRC, i.e. is linear.
        /// </summary>
        Linear = 1 << 10,
        /// <summary>
        /// The data has a TRC - the TRC from the configured space.
        /// </summary>
        Nonlinear = 1 << 11,
        /// <summary>
        /// The data has a TRC - a perceptual TRC where 50% gray is 0.5.
        /// </summary>
        Perceptual = 1 << 12,

        /// <summary>
        /// This is a gray component model.
        /// </summary>
        Gray = 1 << 20,
        /// <summary>
        /// This is an RGB based component model, the space associated is expected to contain an RGB matrix profile.
        /// </summary>
        Rgb = 1 << 21,
        /// <summary>
        /// [Not yet implemented]
        /// </summary>
        Spectral = 1 << 22,
        /// <summary>
        /// This model is part of the CIE family of spaces.
        /// </summary>
        Cie = 1 << 23,
        /// <summary>
        /// The encodings described are CMYK encodings, the space associated is expected to contain an CMYK ICC profile.
        /// </summary>
        Cmyk = 1 << 24,
        /// <summary>
        /// [Not yet implemented]
        /// </summary>
        Luz = 1 << 25,
    }
}
