using UnityEngine;
using System.Collections;

public interface IPlayerStateReadOnly
{
    bool FacingRight         { get; }
    bool RidingFastPlatform  { get; }
    bool TouchingWall        { get; }
    bool Dead                { get; }
    bool LevelCompleted      { get; }
    bool AboveGround         { get; }
}

public interface IPlayerStateFullAccess
{
    bool FacingRight         { get; set; }
    bool RidingFastPlatform  { get; set; }
    bool TouchingWall        { get; set; }
    bool Dead                { get; set; }
    bool LevelCompleted      { get; set; }
    bool AboveGround         { get; set; }
}
