namespace babl
{
    internal abstract class BablFormulaTrc : BablTrc
    {
        protected float[] lut;
        protected BablFormulaTrc(string name, double gamma, float[] lut)
            : base(name, gamma)
        {
            this.lut = lut;
        }

        unsafe protected void NonGammaCtor()
        {
            gammaToLinearX0 = lut[4];
            gammaToLinearX1 = GammaX1;
            fixed (BablPolynomial* to = &gammaToLinear)
                BablPolynomial.ApproximateGamma(to, gamma, gammaToLinearX0, gammaToLinearX1, GammaDegree, GammaScale);

            gammaFromLinearX0 = lut[3] * lut[4];
            gammaFromLinearX1 = GammaX1;
            fixed (BablPolynomial* from = &gammaFromLinear)
                BablPolynomial.ApproximateGamma(from, rgamma, gammaFromLinearX0, gammaFromLinearX1, GammaDegree, GammaScale);
        }
    }
}