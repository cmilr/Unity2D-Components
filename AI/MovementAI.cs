using UnityEngine;
using System.Collections;
using Rotorz.Tile;
using Matcha.Extensions;
using Matcha.Lib;

public class MovementAI : CacheBehaviour {

    public enum MovementStyle { Sentinel, Scout, HesitantScout, Wanderer };
    public MovementStyle movementStyle;
    public float movementSpeed = 2f;

    private TileSystem tileSystem;
    private TileData tile;
    private int tileConvertedX;
    private int tileConvertedY;
    private string idleAnimation;
    private string walkAnimation;
    private Transform target;

    // current state
    [HideInInspector]
    public bool currentlyMoving;
    [HideInInspector]
    public bool paused;
    [HideInInspector]
    public int walkingDirection;
    [HideInInspector]
    public GameObject nextTile;

    void Start()
    {
        target        = GameObject.Find(PLAYER).transform;
        tileSystem    = GameObject.Find(TILE_MAP).GetComponent<TileSystem>();
        idleAnimation = name;
        walkAnimation = name + "_WALK";
    }

    void Update()
    {
        // if next tile is null, clamp movement beyond current tile
        nextTile = transform.GetTileBelow(tileSystem, walkingDirection);

        if (nextTile == null)
        {
            GameObject currentTile = transform.GetTileBelow(tileSystem, 0);

            if (walkingDirection == RIGHT)
            {
                if (transform.position.x > currentTile.transform.position.x)
                    transform.position = new Vector3(currentTile.transform.position.x, transform.position.y, transform.position.z);
            }
            else if (walkingDirection == LEFT)
            {
                if (transform.position.x < currentTile.transform.position.x)
                    transform.position = new Vector3(currentTile.transform.position.x, transform.position.y, transform.position.z);
            }

            rigidbody2D.velocity = Vector2.zero;
        }
    }

    // MASTER CONTROLLER
    void OnBecameVisible()
    {
        switch (movementStyle)
        {
            case MovementStyle.Sentinel:
                InvokeRepeating("LookAtTarget", 1f, .2f);
            break;

            case MovementStyle.Scout:
                InvokeRepeating("FollowTarget", 1f, .01f);
                InvokeRepeating("LookAtTargetWhenPaused", 1f, .2f);
            break;
        }
    }

    void LookAtTarget()
    {
        int direction = (target.position.x > transform.position.x) ? RIGHT : LEFT;
        transform.localScale = new Vector3((float)direction, transform.localScale.y, transform.localScale.z);
    }

    void LookAtTargetWhenPaused()
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
        if (MLib.FloatEqual(transform.position.x, target.position.x, .5f))
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

        yield return new WaitForSeconds(UnityEngine.Random.Range(1, 4));

        InvokeRepeating("FollowTarget", 1f, .1f);
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

    void OnDisable()
    {
        CancelInvoke();
    }
}
