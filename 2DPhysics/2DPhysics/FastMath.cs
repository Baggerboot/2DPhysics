using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2DPhysics
{
	public static class FastMath
	{
		public static float Square(float value)
		{
			return value * value;
		}
		public static float Pow(float value, int exponent)
		{
			float result = value;
			for (int i = 1; i < exponent; i++) {
				result *= value;
			}
			return result;
		}
	}
}
