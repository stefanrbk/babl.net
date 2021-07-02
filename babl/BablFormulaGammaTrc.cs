using System;

namespace babl
{
    class BablFormulaGammaTrc : BablFormulaTrc
    {
        unsafe internal BablFormulaGammaTrc(string name, double gamma)
            : base(name, gamma, Array.Empty<float>())
        {
            gammaToLinearX0 = GammaX0;
            gammaToLinearX1 = GammaX1;
            fixed (BablPolynomial* to = &gammaToLinear)
                BablPolynomial.ApproximateGamma(to, gamma, gammaToLinearX0, gammaToLinearX1, GammaDegree, GammaScale);

            gammaFromLinearX0 = GammaX0;
            gammaFromLinearX1 = GammaX1;
            fixed (BablPolynomial* from = &gammaFromLinear)
                BablPolynomial.ApproximateGamma(from, rgamma, gammaFromLinearX0, gammaFromLinearX1, GammaDegree, GammaScale);
        }
        public override float FromLinear(float val) =>
            GammaFromLinear(val);

        public override float ToLinear(float val) =>
            GammaToLinear(val);

        public static BablTrc New(string name, double gamma) => 
            new BablFormulaGammaTrc(name == "" ? $"trc-formula-gamma-{gamma}" : name, gamma);
    }
}
