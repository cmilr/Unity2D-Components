using UnityEngine;
using System.Collections;

public class PlayerState : BaseBehaviour {

	// player state
	public bool FacingRight 		{ get; set; }
	public bool RidingFastPlatform 	{ get; set; }                   
	public bool TouchingWall 		{ get; set; }
	public bool Dead 				{ get; set; }
}
