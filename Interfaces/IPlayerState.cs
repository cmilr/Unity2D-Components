using UnityEngine;
using System.Collections;

public interface IPlayerStateReadOnly
{
    bool FacingRight            { get; }
    bool RidingFastPlatform     { get; }
    bool TouchingWall           { get; }
    bool Dead                   { get; }
    bool AboveGround            { get; }
    bool Grounded               { get; }
    float PreviousX             { get; }
    float PreviousY             { get; }
    float X();
    float Y();
}

public interface IPlayerStateFullAccess
{
    bool FacingRight            { get; set; }
    bool RidingFastPlatform     { get; set; }
    bool TouchingWall           { get; set; }
    bool Dead                   { get; set; }
    bool AboveGround            { get; set; }
    bool Grounded               { get; set; }
    float PreviousX             { get; set; }
    float PreviousY             { get; set; }
    float X();
    float Y();
}
