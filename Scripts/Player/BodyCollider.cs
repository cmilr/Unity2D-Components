using UnityEngine;
using System.Collections;
using Matcha.Lib;

[RequireComponent(typeof(BoxCollider2D))]

public class BodyCollider : CacheBehaviour
{
    private int layer;
    private int hitFrom;
    private PlayerManager player;
    private IPlayerStateFullAccess state;
    private IGameStateReadOnly game;
    private Weapon enemyWeapon;
    private CreatureEntity enemy;

    void Start()
    {
        player = transform.parent.GetComponent<PlayerManager>();
        state = transform.parent.GetComponent<IPlayerStateFullAccess>();
        game = GameObject.Find(GAME_STATE).GetComponent<IGameStateReadOnly>();
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        layer = coll.gameObject.layer;

        if (layer == ENEMY_WEAPON)
        {
            enemyWeapon = coll.GetComponent<Weapon>();

            if (!enemyWeapon.alreadyCollided && !game.LevelLoading && !state.Dead)
            {
                hitFrom = M.HorizSideThatWasHit(gameObject, coll);

                if (enemyWeapon.weaponType == Weapon.WeaponType.Hammer ||
                    enemyWeapon.weaponType == Weapon.WeaponType.Dagger ||
                    enemyWeapon.weaponType == Weapon.WeaponType.MagicProjectile)
                {
                    enemyWeapon.alreadyCollided = true;

                    player.TakesHit("projectile", enemyWeapon, coll, hitFrom);
                }
            }
        }
        else if (layer == ENEMY_COLLIDER)
        {
            enemy = coll.GetComponent<CreatureEntity>();

            if (!enemy.alreadyCollided && !game.LevelLoading && !state.Dead)
            {
                hitFrom = M.HorizSideThatWasHit(gameObject, coll);

                if (enemy.entityType == CreatureEntity.EntityType.Enemy)
                {
                    enemy.alreadyCollided = true;

                    // player.TouchesEnemy("touch", enemy, coll, hitFrom);
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        layer = coll.gameObject.layer;

        if (layer == ENEMY_WEAPON)
        {
            enemyWeapon = coll.GetComponent<Weapon>();

            enemyWeapon.alreadyCollided = false;
        }
        else if (layer == ENEMY_COLLIDER)
        {
            enemy = coll.GetComponent<CreatureEntity>();

            enemy.alreadyCollided = false;

        }
    }
}