using System;

using static System.Math;
namespace babl
{
    internal static partial class NewtonPow
    {
		public const double LN2 = 0.69314718055994530941723212145818;

		/* a^b = exp(b*log(a))
		 *
		 * Extracting the exponent from a float gives us an approximate log.
		 * Or better yet, reinterpret the bitpattern of the whole float as an int.
		 *
		 * However, the output values of 12throot vary by less than a factor of 2
		 * over the domain we care about, so we only get log() that way, not exp().
		 *
		 * Approximate exp() with a low-degree polynomial; not exactly equal to the
		 * Taylor series since we're minimizing maximum error over a certain finite
		 * domain. It's not worthwhile to use lots of terms, since Newton's method
		 * has a better convergence rate once you get reasonably close to the answer.
		 */
		public static double InitNewton(double x, double exponent, double c0, double c1, double c2)
        {
			(_, var iexp, var y) = Frexp(x);
			y = 2 * y + (iexp - 2);
			c1 *= LN2 * exponent;
			c2 *= LN2 * LN2 * exponent * exponent;
			return c0 + c1 * y + c2 * y * y;
        }

		/// <summary>
		/// Returns x^2.4 == (x*(x^(-1/5)))^3, using Newton's method for x^(-1/5).
		/// </summary>
		public static double Pow24(double x)
        {
			if (x > 16.0)
				// for large values, fall back to a slower but more accurate version
				return Exp(Log(x) * 2.4);
			var y = InitNewton(x, -1.0 / 5, 0.9953189663, 0.9594345146, 0.6742970332);
			for (var i = 0; i < 3; i++)
				y = (1 + 1.0 / 5) * y - ((1.0 / 5) * x * (y * y)) * ((y * y) * (y * y));
			x *= y;
			return x * x * x;
        }
		public static double Pow24Receip(double x)
        {
			if (x > 1024.0)
				// for large values, fall back to a slower but more accurate version
				return Exp(Log(x) * (1 / 2.4));
			var y = InitNewton(x, -1.0 / 12, 0.9976800269, 0.9885126933, 0.5908575383);
			x = Sqrt(x);
			// Newton's method for x^(-1/6)
			var z = (1.0/6) * x;
			for (var i = 0; i < 3; i++)
				y = (7.0 / 6) * y - z * ((y * y) * (y * y) * (y * y * y));
			return x * y;
		}
		public static (bool negative, sbyte exponent, double mantissa) Frexp(double value)
		{
			var iValue = BitConverter.DoubleToInt64Bits(value);
			var ee = iValue >> 52 & 0x7FF;
			long e;

			if (ee is 0)
			{
				if (iValue is not 0)
				{
					(_, e, value) = Frexp(value * 1.3407807929942597E+154);
					iValue = BitConverter.DoubleToInt64Bits(value);
					e -= 64;
				}
				else
					e = 0;
				return (double.IsNegative(value), (sbyte)e, BitConverter.Int64BitsToDouble(iValue));
			}
			else if (ee is 0x7FF)
				return (double.IsNegative(value), 0, value);
			e = ee - 0x3fe;
			iValue &= ~0x7ff0_0000_0000_0000;
			iValue |= 0x3fe0_0000_0000_0000;
			return (double.IsNegative(value), (sbyte)e, BitConverter.Int64BitsToDouble(iValue));
		}
	}
}
