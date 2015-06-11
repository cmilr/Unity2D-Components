using UnityEngine;
using System.Collections;


// a clearinghouse for temporary player state
public class PlayerState : BaseBehaviour, IPlayerStateReadOnly, IPlayerStateFullAccess
{
	// player state
	public string Character 		{ get; set; }
	public bool FacingRight 		{ get; set; }
	public bool RidingFastPlatform 	{ get; set; }
	public bool TouchingWall 		{ get; set; }
	public bool Dead 				{ get; set; }
	public bool AboveGround 		{ get; set; }
	public bool Grounded 	 		{ get; set; }
	public float PreviousX			{ get; set; }
	public float PreviousY			{ get; set; }
	public float X					{ get; set; }
	public float Y					{ get; set; }
    public int HitFrom              { get; set; }

	void OnEnable()
	{
		Character = "LAURA";
        FacingRight = true;
		Messenger.AddListener<bool>("touching wall", OnTouchingWall);
		Messenger.AddListener<bool>("riding fast platform", OnRidingFastPlatform);
		Messenger.AddListener<bool>("player above ground", OnPlayerAboveGround);
		Messenger.AddListener<string, Collider2D, int>("player dead", OnPlayerDead);
	}

	void OnDestroy()
	{
		Messenger.RemoveListener<bool>("touching wall", OnTouchingWall);
		Messenger.RemoveListener<bool>("riding fast platform", OnRidingFastPlatform);
		Messenger.RemoveListener<bool>("player above ground", OnPlayerAboveGround);
		Messenger.RemoveListener<string, Collider2D, int>("player dead", OnPlayerDead);
	}

	void OnTouchingWall(bool status)
	{
		TouchingWall = status;
	}

	void OnRidingFastPlatform(bool status)
	{
		RidingFastPlatform = status;
	}

	void OnPlayerAboveGround(bool status)
	{
		AboveGround = status;
	}

	void OnPlayerDead (string methodOfDeath, Collider2D coll, int hitFrom)
	{
		Dead = true;
        HitFrom = hitFrom;
	}
}


public interface IPlayerStateFullAccess {

    string Character            { get; set; }
    bool FacingRight            { get; set; }
    bool RidingFastPlatform     { get; set; }
    bool TouchingWall           { get; set; }
    bool Dead                   { get; set; }
    bool AboveGround            { get; set; }
    bool Grounded               { get; set; }
    float PreviousX             { get; set; }
    float PreviousY             { get; set; }
    float X                     { get; set; }
    float Y						{ get; set; }
}


public interface IPlayerStateReadOnly {

    string Character            { get; }
    bool FacingRight            { get; }
    bool RidingFastPlatform     { get; }
    bool TouchingWall           { get; }
    bool Dead                   { get; }
    bool AboveGround            { get; }
    bool Grounded               { get; }
    float PreviousX             { get; }
    float PreviousY             { get; }
    float X                     { get; }
    float Y                     { get; }
}