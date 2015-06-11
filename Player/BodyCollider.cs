using UnityEngine;
using System.Collections;
using Matcha.Lib;

[RequireComponent(typeof(BoxCollider2D))]

public class BodyCollider : CacheBehaviour
{
    private int layer;
    private int hitFrom;
    private IPlayerStateFullAccess state;
    private IGameStateReadOnly game;
    private Weapon enemyWeapon;
    private CreatureEntity enemy;

    void Start()
    {
        state = transform.parent.GetComponent<IPlayerStateFullAccess>();
        game = GameObject.Find(GAME_STATE).GetComponent<IGameStateReadOnly>();
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        // check for layer instead of name — it's much quicker
        layer = coll.gameObject.layer;

        if (layer == ENEMY_WEAPON)
        {
            enemyWeapon = coll.GetComponent<Weapon>();

            if (!enemyWeapon.alreadyCollided && !game.LevelLoading && !state.Dead)
            {
                hitFrom = MLib.HorizSideThatWasHit(gameObject, coll);

                if (enemyWeapon.weaponType == Weapon.WeaponType.Projectile)
                {
                    enemyWeapon.alreadyCollided = true;

                    Messenger.Broadcast<string, Collider2D, int>("player dead", "projectile", coll, hitFrom);
                }
            }
        }
        else if (layer == ENEMY_COLLIDER)
        {
            enemy = coll.GetComponent<CreatureEntity>();

            if (!enemy.alreadyCollided && !game.LevelLoading && !state.Dead)
            {
                hitFrom = MLib.HorizSideThatWasHit(gameObject, coll);

                if (enemy.entityType == CreatureEntity.EntityType.Enemy)
                {
                    enemy.alreadyCollided = true;

                    Messenger.Broadcast<string, Collider2D, int>("player dead", "struckdown", coll, hitFrom);
                }
            }
        }
    }
}