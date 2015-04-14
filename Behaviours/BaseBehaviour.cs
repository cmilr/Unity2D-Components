﻿//       __    __     ______     ______   ______     __  __     ______
//      /\ "-./  \   /\  __ \   /\__  _\ /\  ___\   /\ \_\ \   /\  __ \
//      \ \ \-./\ \  \ \  __ \  \/_/\ \/ \ \ \____  \ \  __ \  \ \  __ \
//       \ \_\ \ \_\  \ \_\ \_\    \ \_\  \ \_____\  \ \_\ \_\  \ \_\ \_\
//        \/_/  \/_/   \/_/\/_/     \/_/   \/_____/   \/_/\/_/   \/_/\/_/
//         I  N  D  U  S  T  R  I  E  S             www.matcha.industries

using UnityEngine;
using System.Collections;


public class BaseBehaviour : MonoBehaviour {

    // global stuff
    protected string character                  = "LAURA";
    protected const float ONE_PIXEL             = .03125f;
    protected const float ONE_HUD_PIXEL         = 5f;
    protected const int TOP                     = 0;
    protected const int BOTTOM                  = 1;
    protected const int LEFT                    = 2;
    protected const int RIGHT                   = 3;

    // gameObject names
    protected const string PLAYER               = "Player";
    protected const string _GAME_DATA           = "_GameData";
    protected const string GAME_STATE           = "GameState";
    protected const string TILE_MAP             = "TileMap";
    protected const string DEFAULT_TILE         = "DefaultTile";

    // layer names
    protected const int WEAPON_COLLIDER         = 9;
    protected const int BODY_COLLIDER           = 10;

    // player offsets
    protected const float ABOUTFACE_OFFSET      = .2f;

    // player/weapon animation speeds
    protected const float IDLE_SPEED            = 1f;
    protected const float RUN_SPEED             = .5f;
    protected const float JUMP_SPEED            = 10f;
    protected const float SWING_SPEED           = 1f;

    // entity auto-alignment settings
    protected const float ALIGN_ENTITY_TO       = .124f;

    // hud specs
    protected const float HUD_TOP_MARGIN        = 100f;
    protected const float HUD_HEARTS_TOP_MARGIN = 17f;
    protected const float HUD_Z                 = 10f;
    protected const float HUD_FADE_IN_AFTER     = .75f;
    protected const float HUD_FADE_OUT_AFTER    = .25f;


    protected void Dbug()
    {
        Debug.Log(">>>>>>>> DEBUG METHOD CALLED <<<<<<<<");
    }
}