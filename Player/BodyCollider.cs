using UnityEngine;
using System.Collections;
using Matcha.Lib;

[RequireComponent(typeof(BoxCollider2D))]

public class BodyCollider : CacheBehaviour
{
    private int layer;
    private IPlayerStateFullAccess state;
    private IGameStateReadOnly game;

    void Start()
    {
        state = transform.parent.GetComponent<IPlayerStateFullAccess>();
        game = GameObject.Find(GAME_STATE).GetComponent<IGameStateReadOnly>();
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        // check for layer instead of name — it's much quicker
        layer = coll.gameObject.layer;

        if (layer == ENEMY_WEAPON && !coll.GetComponent<Weapon>().alreadyCollided && !game.LevelLoading && !state.Dead)
        {
            coll.GetComponent<Weapon>().alreadyCollided = true;
            Messenger.Broadcast<string, Collider2D>("player dead", "struckdown", coll);
        }
    }
}