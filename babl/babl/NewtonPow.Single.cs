using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.MathF;
namespace babl
{
    internal static partial class NewtonPow
    {
        public const float LN2f = 0.69314718055994530941723212145818f;

		public static float InitNewton(float x, float exponent, float c0, float c1, float c2)
		{
			(_, var iexp, var y) = Frexpf(x);
			y = 2 * y + (iexp - 2);
			c1 *= LN2f * exponent;
			c2 *= LN2f * LN2f * exponent * exponent;
			return c0 + c1 * y + c2 * y * y;
		}

		/// <summary>
		/// Returns x^2.4 == (x*(x^(-1/5)))^3, using Newton's method for x^(-1/5).
		/// </summary>
		public static float Pow24(float x)
		{
			if (x > 16.0)
				// for large values, fall back to a slower but more accurate version
				return Exp(Log(x) * 2.4f);
			var y = InitNewton(x, -1.0f / 5, 0.9953189663f, 0.9594345146f, 0.6742970332f);
			for (var i = 0; i < 3; i++)
				y = (1 + 1.0f / 5) * y - ((1.0f / 5) * x * (y * y)) * ((y * y) * (y * y));
			x *= y;
			return x * x * x;
		}
		public static float Pow24Receip(float x)
		{
			if (x > 1024.0)
				// for large values, fall back to a slower but more accurate version
				return Exp(Log(x) * (1 / 2.4f));
			var y = InitNewton(x, -1.0f / 12, 0.9976800269f, 0.9885126933f, 0.5908575383f);
			x = Sqrt(x);
			// Newton's method for x^(-1/6)
			var z = (1.0f / 6) * x;
			for (var i = 0; i < 3; i++)
				y = (7.0f / 6) * y - z * ((y * y) * (y * y) * (y * y * y));
			return x * y;
		}
		public static (bool negative, sbyte exponent, float mantissa) Frexpf(float value)
		{
			var iValue = BitConverter.SingleToInt32Bits(value);
			var ee = iValue >> 23 & 0xFF;
			int e;

			if (ee is 0)
			{
				if (iValue is not 0)
				{
					(_, e, value) = Frexpf(value * 18446744073709551616.0f);
					iValue = BitConverter.SingleToInt32Bits(value);
					e -= 64;
				}
				else
					e = 0;
				return (float.IsNegative(value), (sbyte)e, BitConverter.Int32BitsToSingle(iValue));
			}
			else if (ee is 0xFF)
				return (float.IsNegative(value), 0, value);
			e = ee - 0x7e;
			iValue &= ~0x7f800000;
			iValue |= 0x3f000000;
			return (float.IsNegative(value), (sbyte)e, BitConverter.Int32BitsToSingle(iValue));
		}
	}
}
