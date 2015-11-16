//       __    __     ______     ______   ______     __  __     ______
//      /\ "-./  \   /\  __ \   /\__  _\ /\  ___\   /\ \_\ \   /\  __ \
//      \ \ \-./\ \  \ \  __ \  \/_/\ \/ \ \ \____  \ \  __ \  \ \  __ \
//       \ \_\ \ \_\  \ \_\ \_\    \ \_\  \ \_____\  \ \_\ \_\  \ \_\ \_\
//        \/_/  \/_/   \/_/\/_/     \/_/   \/_____/   \/_/\/_/   \/_/\/_/
//         I  N  D  U  S  T  R  I  E  S             www.matcha.industries

using Matcha.Unity;
using System.Collections;
using UnityEngine;

namespace Matcha.Dreadful
{
	public class MCLR : BaseBehaviour
	{
		// weapon colors
		public static Color32 white             = M.HexToColor("ffffff");
		public static Color32 black             = M.HexToColor("000000");
		public static Color32 orange            = M.HexToColor("fa8419");
		public static Color32 bloodRed          = M.HexToColor("eb0308");
		public static Color32 bloodPink         = M.HexToColor("d16c6f");
		public static Color32 lightBrown        = M.HexToColor("4b2e00");
		public static Color32 darkBrown         = M.HexToColor("371f05");
		public static Color32 undergroundBrown  = M.HexToColor("311800");
		public static Color32 deepBrightBlue    = M.HexToColor("0d04c8");
		public static Color32 deepPurple        = M.HexToColor("6c07ac");

		// default sword
		public static Color32 defaultGrayBlade  = M.HexToColor("ffffff");
		public static Color32 defaultGrayHandle = M.HexToColor("949292");
	}
}
