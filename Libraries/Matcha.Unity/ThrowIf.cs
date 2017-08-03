using System;

namespace Matcha.Unity
{
	static class ThrowIf
	{
		public static class Argument
		{
			public static void IsNull(object argument)
			{
				if (argument == null || argument.Equals(null))
				{
					throw new ArgumentNullException(argument.ToString());
				}
			}
		}

		public static class Reference
		{
			public static void IsNull(object reference)
			{
				if (reference == null || reference.Equals(null))
				{
					throw new ArgumentNullException(reference.ToString());
				}
			}
		}
	}
}

