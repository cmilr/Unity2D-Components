//       __    __     ______     ______   ______     __  __     ______
//      /\ "-./  \   /\  __ \   /\__  _\ /\  ___\   /\ \_\ \   /\  __ \
//      \ \ \-./\ \  \ \  __ \  \/_/\ \/ \ \ \____  \ \  __ \  \ \  __ \
//       \ \_\ \ \_\  \ \_\ \_\    \ \_\  \ \_____\  \ \_\ \_\  \ \_\ \_\
//        \/_/  \/_/   \/_/\/_/     \/_/   \/_____/   \/_/\/_/   \/_/\/_/
//         I  N  D  U  S  T  R  I  E  S       http://jguarshark.github.io

using UnityEngine;
using System.Collections;


public class BaseBehaviour : MonoBehaviour {

    // global stuff
    protected const int TOP                             = 0;
    protected const int BOTTOM                          = 1;
    protected const int LEFT                            = 2;
    protected const int RIGHT                           = 3;
    protected const int UP                              = 4;
    protected const int DOWN                            = 5;

    // global measurements
    protected const float ONE_PIXEL                     = .03125f;
    protected const float ONE_HUD_PIXEL                 = 5f;
    protected const float ONE_COLLIDER_PIXEL            = .62f;

    // gameObject names
    protected const string PLAYER                       = "Player";
    protected const string _PLAYER_DATA                 = "_PlayerData";
    protected const string _GAME_DATA                   = "_GameData";
    protected const string GAME_STATE                   = "GameState";
    protected const string TILE_MAP                     = "TileMap";
    protected const string DEFAULT_TILE                 = "DefaultTile";

    // layer names for colliders
    protected const int PLAYER_COLLIDER                 = 12;
    protected const int WEAPON_COLLIDER                 = 9;
    protected const int BODY_COLLIDER                   = 10;

    // player offsets
    protected const float ABOUTFACE_OFFSET              = .15f;

    // player/weapon animation speeds
    protected const float IDLE_SPEED                    = 1f;
    protected const float RUN_SPEED                     = .5f;
    protected const float JUMP_SPEED                    = 10f;
    protected const float SWING_SPEED                   = 1f;

    // player/weapon states
    protected const int IDLE                            = 0;
    protected const int RUN                             = 1;
    protected const int JUMP                            = 2;
    protected const int FALL                            = 3;
    protected const int ATTACK                          = 4;
    protected const int RUN_ATTACK                      = 5;
    protected const int JUMP_ATTACK                     = 6;


    // player/death animation speeds
    protected const float STRUCKDOWN_SPEED              = 1f;
    protected const float DROWNED_SPEED                 = 8f;

    // entity auto-alignment settings
    protected const float ALIGN_ENTITY_TO               = .124f;

    // hud specs
    protected const float HUD_WEAPON_TOP_MARGIN         = 125f;
    protected const float HUD_STASHED_WEAPON_OFFSET     = 85f;
    protected const float HUD_STASHED_TRANSPARENCY      = .25f;
    protected const float HUD_HEARTS_TOP_MARGIN         = 17f;
    protected const float HUD_Z                         = 10f;
    protected const float HUD_FADE_IN_AFTER             = .75f;
    protected const float HUD_FADE_OUT_AFTER            = .25f;
    protected const float HUD_INITIAL_TIME_TO_FADE      = 2f;
    protected const float HUD_WEAPON_CHANGE_FADE        = 0f;


    protected void Dbug()
    {
        Debug.Log(">>>>>>>> DEBUG METHOD CALLED <<<<<<<<");
    }
}