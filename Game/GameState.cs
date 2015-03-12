using UnityEngine;
using System.Collections;


// a clearinghouse for temporary game state
public class GameState : BaseBehaviour, IGameStateReadOnly, IGameStateFullAccess
{
    // game state
    public bool LevelLoading      { get; set; }

    void OnEnable()
    {
        Messenger.AddListener<int>( "load level", OnLoadLevel);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener<int>( "load level", OnLoadLevel);
    }

    void OnLoadLevel(int unused)
    {
        LevelLoading = true;
    }
}