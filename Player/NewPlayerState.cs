//       __    __     ______     ______   ______     __  __     ______
//      /\ "-./  \   /\  __ \   /\__  _\ /\  ___\   /\ \_\ \   /\  __ \
//      \ \ \-./\ \  \ \  __ \  \/_/\ \/ \ \ \____  \ \  __ \  \ \  __ \
//       \ \_\ \ \_\  \ \_\ \_\    \ \_\  \ \_____\  \ \_\ \_\  \ \_\ \_\
//        \/_/  \/_/   \/_/\/_/     \/_/   \/_____/   \/_/\/_/   \/_/\/_/
//         I  N  D  U  S  T  R  I  E  S             www.matcha.industries

using System;
using UnityEngine;

public static class NewPlayerState : object
{
	public static string Character                     { get; set; }
	public static bool RidingFastPlatform              { get; set; }
	public static bool TouchingWall                    { get; set; }
	public static bool Dead                            { get; set; }
	public static bool AboveGround                     { get; set; }
	public static bool Grounded                        { get; set; }
	public static bool MovingHorizontally              { get; set; }
	public static float PreviousX                      { get; set; }
	public static float PreviousY                      { get; set; }
	public static float X                              { get; set; }
	public static float Y                              { get; set; }
	public static int HitFrom                          { get; set; }
}
