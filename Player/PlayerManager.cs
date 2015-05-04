using UnityEngine;
using System.Collections;


public class PlayerManager : BaseBehaviour
{
    private _PlayerData playerData;

    void Start()
    {
        playerData = GameObject.Find(_PLAYER_DATA).GetComponent<_PlayerData>();

        Init();
    }

    void Init()
    {
        Messenger.Broadcast<int>("init lvl", playerData.LVL);
        Messenger.Broadcast<int>("init hp", playerData.HP);
        Messenger.Broadcast<int>("init ac", playerData.AC);
        Messenger.Broadcast<int>("init xp", playerData.XP);
        Messenger.Broadcast<GameObject, GameObject, GameObject>
            ("init weapons", playerData.equippedWeapon, playerData.leftWeapon, playerData.rightWeapon);
    }

    void OnPrizeCollected(int worth)
    {
        // gameData.CurrentScore += worth;
        // Messenger.Broadcast<int>("change score", gameData.CurrentScore);
    }

    void OnLevelCompleted(bool status)
    {
        // gameData.LastSavedScore = gameData.CurrentScore;
        // gameData.CurrentLevel = gameData.CurrentLevel;
        // Messenger.Broadcast<bool>("fade hud", true);
        // Messenger.Broadcast<int>("load level", gameData.CurrentLevel);
    }

    void OnEnable()
    {
        Messenger.AddListener<int>( "prize collected", OnPrizeCollected);
        Messenger.AddListener<bool>( "level completed", OnLevelCompleted);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener<int>( "prize collected", OnPrizeCollected );
        Messenger.RemoveListener<bool>( "level completed", OnLevelCompleted);
    }
}