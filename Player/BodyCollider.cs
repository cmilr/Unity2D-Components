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
                hitFrom = HorizontalCollisionSide(coll);

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
                hitFrom = HorizontalCollisionSide(coll);

                if (enemy.entityType == CreatureEntity.EntityType.Enemy)
                {
                    enemy.alreadyCollided = true;

                    Messenger.Broadcast<string, Collider2D, int>("player dead", "struckdown", coll, hitFrom);
                }
            }
        }
    }

    private int HorizontalCollisionSide(Collider2D coll)
    {
        Vector3 relativePosition = transform.InverseTransformPoint(coll.transform.position);

        // if scale is positive: ie, facing right
        if (transform.lossyScale.x == 1)
        {
            if (relativePosition.x > 0)
            {
                return RIGHT;
            }
            else
            {
                return LEFT;
            }
        }
        // if scale is negative: ie, facing left
        else if (transform.lossyScale.x == -1)
        {
            if (relativePosition.x < 0)
            {
                return RIGHT;
            }
            else
            {
                return LEFT;
            }
        }

        // else default
        else
        {
            return RIGHT;
        }
    }

    private int VerticalCollisionSide(Collider2D coll)
    {
        Vector3 relativePosition = transform.InverseTransformPoint(coll.transform.position);

        if (relativePosition.y > 0)
        {
            return TOP;
        }
        else
        {
            return BOTTOM;
        }
    }
}