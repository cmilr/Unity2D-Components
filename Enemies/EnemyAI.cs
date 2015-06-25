using Matcha.Game.Colors;   // testing only
using UnityEngine;
using System;
using System.Collections;
using Rotorz.Tile;
using Matcha.Extensions;
using Matcha.Lib;

public class EnemyAI : CacheBehaviour {

    public float attackInterval    = 1f;
    public float chanceOfAttack    = 40f;
    public float attackWhenInRange = 20f;
    public float movementSpeed = 2f;

    private TileSystem tileSystem;
    private TileData tile;
    private ProjectileManager projectile;
    private Weapon weapon;
    private int tileConvertedX;
    private int tileConvertedY;
    private string idleAnimation;
    private string walkAnimation;
    private Transform target;

    // current state
    // private bool facingRight;
    private bool currentlyMoving;
    private bool paused;
    private int walkingDirection;

	void Start()
    {
        projectile    = GetComponent<ProjectileManager>();
        weapon        = GetComponentInChildren<Weapon>();
        target        = GameObject.Find(PLAYER).transform;
        tileSystem    = GameObject.Find(TILE_MAP).GetComponent<TileSystem>();

        SetAnimations();
	}

    void SetAnimations()
    {
        idleAnimation = name;
        walkAnimation = name + "_WALK";
    }

    void OnBecameVisible()
    {
        MasterController();

        if (test) {
            StartCoroutine(LobCompTest());
        } else {
            InvokeRepeating("AttackRandomly", 2f, attackInterval);
        }
    }

    void MasterController()
    {
        InvokeRepeating("LookAtTarget", 1f, .2f);
        InvokeRepeating("FollowTarget", 1f, .1f);
    }

    void LookAtTarget()
    {
        if (paused)
        {
            int direction = (target.position.x > transform.position.x) ? RIGHT : LEFT;
            transform.localScale = new Vector3((float)direction, transform.localScale.y, transform.localScale.z);
        }
    }

    void FollowTarget()
    {
        // if actor and target are on roughly same x axis, pause actor
        if (MLib.FloatEqual(transform.position.x, target.position.x, .2f))
        {
            paused = true;
            currentlyMoving = false;
        }
        // otherwise, let's get the proper direction for the actor to move
        else
        {
            walkingDirection = (target.position.x > transform.position.x) ? RIGHT : LEFT;
        }

        // if not intentionally paused, start moving!
        if (!currentlyMoving && !paused)
        {
            currentlyMoving = true;

            // ensure that actor is always facing in the direction it is moving

            transform.localScale = new Vector3((float)walkingDirection, transform.localScale.y, transform.localScale.z);
            rigidbody2D.velocity = transform.right * movementSpeed * walkingDirection;

            animator.speed = ENEMY_WALK_SPEED;
            animator.Play(Animator.StringToHash(walkAnimation));
        }
        else if (paused)
        {
            rigidbody2D.velocity = Vector2.zero;

            animator.speed = ENEMY_IDLE_SPEED;
            animator.Play(Animator.StringToHash(idleAnimation));
        }

        GameObject nextTile = transform.GetTileBelow(tileSystem, walkingDirection * 2);

        if (nextTile == null)
        {
            paused = true;
            currentlyMoving = false;
        }
        else
        {
            paused = false;
            currentlyMoving = false;
        }

        // random pauses
        if (UnityEngine.Random.Range(0, 201) <= 1){
            rigidbody2D.velocity = Vector2.zero;
            StartCoroutine(PauseFollowTarget());
        }
    }

    IEnumerator PauseFollowTarget()
    {
        CancelInvoke("FollowTarget");

        Debug.Log("Pause Called");

        yield return new WaitForSeconds(UnityEngine.Random.Range(1, 4));

        InvokeRepeating("FollowTarget", 1f, .1f);
    }

    // void Walk()
    // {
    //     // if not intentionally stopped, start moving!
    //     if (!moving && !paused && !edgeReached)
    //     {
    //         // ensure that enemy is always facing in the direction it is moving
    //         transform.localScale = new Vector3((float)walkingDirection, transform.localScale.y, transform.localScale.z);
    //         rigidbody2D.velocity = transform.right * movementSpeed * walkingDirection;
    //         moving = true;
    //     }
    //     else if (edgeReached)
    //     {
    //         rigidbody2D.velocity = Vector2.zero;
    //     }

    //     // stop when reaching edge of platform
    //     GameObject obj = transform.GetTileBelow(tileSystem, walkingDirection);
    //     if (obj == null)
    //     {
    //         edgeReached = true;
    //         moving = false;
    //     }
    // }

    void AttackRandomly()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= attackWhenInRange)
        {
            if (UnityEngine.Random.Range(1, 101) <= chanceOfAttack)
                projectile.FireAtTarget(weapon, target);
        }
    }

    void RotateTowardsTarget()
    {
        Vector3 vel = GetForceFrom(transform.position,target.position);
        float angle = Mathf.Atan2(vel.y, vel.x)* Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle);
    }

    Vector2 GetForceFrom(Vector3 fromPos, Vector3 toPos)
    {
        float power = 1;
        return (new Vector2(toPos.x, toPos.y) - new Vector2(fromPos.x, fromPos.y))*power;
    }

    void OnBecameInvisible()
    {
        CancelInvoke("LookAtTarget");

        if (!test)
            CancelInvoke("AttackRandomly");
    }

    void OnDisable()
    {
        CancelInvoke();
    }





    public bool test;
    public GameObject[] targets;    // testing only



    // TARGET TESTING SUITE
    // ####################

    IEnumerator LobCompTest()
    {
        int i = 0;
        int j = i - 1;

        while (true)
        {
            if (i >= targets.Length)
                i = 0;
            if (j >= targets.Length)
                j = 0;
            if (j < 0)
                j = 10;

            targets[i].GetComponent<SpriteRenderer>().material.SetColor("_Color", MColor.orange);
            targets[j].GetComponent<SpriteRenderer>().material.SetColor("_Color", MColor.white);
            projectile.FireAtTarget(weapon, targets[i].transform);
            i++;
            j++;
            yield return new WaitForSeconds(2);
        }
    }

}
