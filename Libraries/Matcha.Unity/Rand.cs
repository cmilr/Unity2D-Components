using UnityEngine;

namespace Matcha.Unity
{
	static class Rand
	{
		// returns a random int between min and max (inclusive.)
		public static int Range(int min, int max)
		{
			return Random.Range(min, max + 1);
		}

		// returns a random float between min and max (inclusive).
		public static float Range(float min, float max)
		{
			return Random.Range(min, max);
		}

		// returns a gaussian-distributed random int.
		public static int Gaussian(int mean, int standard_deviation)
		{
			return mean + (int)Gaussian() * standard_deviation;
		}

		// returns a gaussian-distributed random int
		// between min and max (inclusive.)
		public static int Gaussian(int mean, int standard_deviation, int min, int max)
		{
			int x;
			do {
				x = Gaussian(mean, standard_deviation);
			} while (x < min || x > max);

			return x;
		}
		
		// returns a gaussian-distributed random float.
		public static float Gaussian(float mean, float standard_deviation)
		{
			return mean + Gaussian() * standard_deviation;
		}

		// returns a gaussian-distributed random float
		// between min and max (inclusive.)
		public static float Gaussian(float mean, float standard_deviation, float min, float max)
		{
			float x;
			do {
				x = Gaussian(mean, standard_deviation);
			} while (x < min || x > max);

			return x;
		}

		// returns a gaussian-distributed float.
		public static float Gaussian()
		{
			float v1, v2, s;
			do {
				v1 = 2.0f * Random.Range(0f,1f) - 1.0f;
				v2 = 2.0f * Random.Range(0f,1f) - 1.0f;
				s = v1 * v1 + v2 * v2;
			} while (s >= 1.0f || s.FloatEquals(0f));

			s = Mathf.Sqrt((-2.0f * Mathf.Log(s)) / s);

			return v1 * s;
		}

		// seeds Rand with the system clock.
		public static void Seed()
		{
			Random.InitState((int)System.DateTime.Now.Ticks);
		}

		// seeds Rand with a supplied int.
		public static void Seed(int value)
		{
			Random.InitState(value);
		}

		// returns a random int between min and max (inclusive,)
		// and divisible by two.
		public static int RangeDivTwo(int min, int max)
		{
			int x = Random.Range(min, max + 1);
			while (x % 2 != 0) { x--; }

			return x;
		}

		// returns a random int between min and max (inclusive,)
		// and divisible by four.
		public static int RangeDivFour(int min, int max)
		{
			int x = Random.Range(min, max + 1);
			while (x % 4 != 0) { x--; }

			return x;
		}

		// returns a gaussian-distributed int between min and max (inclusive,)
		// and divisible by four.
		public static int GaussianDivFour(int mean, int standard_deviation, int min, int max)
		{
			int x;
			do
			{
				x = Gaussian(mean, standard_deviation);
			} while (x < min || x > max);

			while (x % 4 != 0)
			{
				x++;
			}

			return x;
		}
	}
}


