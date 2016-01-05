//       __    __     ______     ______   ______     __  __     ______
//      /\ "-./  \   /\  __ \   /\__  _\ /\  ___\   /\ \_\ \   /\  __ \
//      \ \ \-./\ \  \ \  __ \  \/_/\ \/ \ \ \____  \ \  __ \  \ \  __ \
//       \ \_\ \ \_\  \ \_\ \_\    \ \_\  \ \_____\  \ \_\ \_\  \ \_\ \_\
//        \/_/  \/_/   \/_/\/_/     \/_/   \/_____/   \/_/\/_/   \/_/\/_/
//         I  N  D  U  S  T  R  I  E  S             www.matcha.industries

using System;
using UnityEngine;

namespace Matcha.Unity
{
	internal static class ThrowIf
	{
		public static class Argument
		{
			public static void IsNull(System.Object argument, string argumentName)
			{
				if (argument == null || argument.Equals(null))
				{
					throw new ArgumentNullException(argumentName);
				}
			}
		}
	}
}
