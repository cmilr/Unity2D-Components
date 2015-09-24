//       __    __     ______     ______   ______     __  __     ______
//      /\ "-./  \   /\  __ \   /\__  _\ /\  ___\   /\ \_\ \   /\  __ \
//      \ \ \-./\ \  \ \  __ \  \/_/\ \/ \ \ \____  \ \  __ \  \ \  __ \
//       \ \_\ \ \_\  \ \_\ \_\    \ \_\  \ \_____\  \ \_\ \_\  \ \_\ \_\
//        \/_/  \/_/   \/_/\/_/     \/_/   \/_____/   \/_/\/_/   \/_/\/_/
//         I  N  D  U  S  T  R  I  E  S             www.matcha.industries

using UnityEngine;
using System.Collections;
using Matcha.Lib;


namespace Matcha.Dreadful.Colors {

	public class MColor : BaseBehaviour
	{
        public static Color32 white             = MLib.HexToColor("ffffff");
        public static Color32 black             = MLib.HexToColor("000000");
        public static Color32 orange            = MLib.HexToColor("fa8419");
        public static Color32 bloodRed          = MLib.HexToColor("eb0308");
        public static Color32 bloodPink         = MLib.HexToColor("d16c6f");
        public static Color32 lightBrown        = MLib.HexToColor("4b2e00");
        public static Color32 darkBrown         = MLib.HexToColor("371f05");
        public static Color32 undergroundBrown  = MLib.HexToColor("311800");
        public static Color32 deepBrightBlue    = MLib.HexToColor("0d04c8");
        public static Color32 deepPurple        = MLib.HexToColor("6c07ac");

        // default sword
        public static Color32 defaultGrayBlade  = MLib.HexToColor("ffffff");
        public static Color32 defaultGrayHandle = MLib.HexToColor("949292");
	}
}