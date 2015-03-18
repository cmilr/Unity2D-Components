using UnityEngine;
using System.Collections;


public class BaseBehaviour : MonoBehaviour {

    protected const string PLAYER            = "Player";
    protected const string _GAME_DATA        = "_GameData";
    protected const string GAME_STATE        = "GameState";
    protected const string TILE_MAP          = "TileMap";
    protected const string DEFAULT_TILE      = "DefaultTile";

    protected const float ALIGN_ENTITY_TO    = .124f;
    protected const float HUD_FADE_IN_AFTER  = .75f;
    protected const float HUD_FADE_OUT_AFTER = .25f;

    protected void Dbug()
    {
        Debug.Log(">>>>>>>> DEBUG METHOD CALLED <<<<<<<<");
    }
}