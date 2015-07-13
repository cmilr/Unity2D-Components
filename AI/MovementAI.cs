using UnityEngine;
using System.Collections;
using Rotorz.Tile;
using Matcha.Extensions;
using Matcha.Lib;

public class MovementAI : CacheBehaviour {

    public enum MovementStyle { Sentinel, Scout, HesitantScout, Wanderer };
    public MovementStyle movementStyle;
    public float movementSpeed      = 2f;
    public float walkAnimationSpeed = .5f;
    public float chanceOfPause      = 1f;           // chance of pause during any given interval

    private TileSystem tileSystem;
    private TileData tile;
    private int tileConvertedX;
    private int tileConvertedY;
    private string walkAnimation;
    private float movementInterval;
    private float lookInterval      = .3f;
    private float xAxisOffset       = .3f;
    private bool hesitant;
    private Transform target;

    // current state
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
        walkAnimation = name + "_WALK_";
        animator.speed = walkAnimationSpeed;
        animator.Play(Animator.StringToHash(walkAnimation));

        movementInterval = UnityEngine.Random.Range(.15f, 1f);

        if (movementStyle == MovementStyle.HesitantScout)
            hesitant = true;
    }

    void Update()
    {
        switch (movementStyle)
        {
            case MovementStyle.Scout:
            case MovementStyle.HesitantScout:
                StopCheck();
            break;
        }
    }

    // MASTER CONTROLLER
    void OnBecameVisible()
    {
        switch (movementStyle)
        {
            case MovementStyle.Sentinel:
                InvokeRepeating("LookAtTarget", 1f, lookInterval);
            break;

            case MovementStyle.Scout:
            case MovementStyle.HesitantScout:
                InvokeRepeating("LookAtTarget", 1f, lookInterval);
                InvokeRepeating("FollowTarget", 1f, movementInterval);
            break;
        }
    }

    void LookAtTarget()
    {
        int direction = (target.position.x > transform.position.x) ? RIGHT : LEFT;
        transform.localScale = new Vector3((float)direction, transform.localScale.y, transform.localScale.z);
    }

    void FollowTarget()
    {
        // get the proper direction for the enemy to move, then send him moving
        if (!paused)
        {
            walkingDirection = (target.position.x > transform.position.x) ? RIGHT : LEFT;
            rigidbody2D.velocity = transform.right * movementSpeed * walkingDirection;

            // ensure that actor is always facing in the direction it is moving
            transform.localScale = new Vector3((float)walkingDirection, transform.localScale.y, transform.localScale.z);

            // add some random pauses
            if (hesitant && UnityEngine.Random.Range(0f, 100f) <= chanceOfPause)
            {
                rigidbody2D.velocity = Vector2.zero;
                StartCoroutine(PauseFollowTarget());
            }
        }
    }

    IEnumerator PauseFollowTarget()
    {
        CancelInvoke("FollowTarget");
        yield return new WaitForSeconds(UnityEngine.Random.Range(2, 5));
        InvokeRepeating("FollowTarget", 1f, movementInterval);
    }

    void StopCheck()
    {
        walkingDirection = (target.position.x > transform.position.x) ? RIGHT : LEFT;

        nextTile = transform.GetTileBelow(tileSystem, walkingDirection);

        if (nextTile == null)
        {
            GameObject currentTile = transform.GetTileBelow(tileSystem, 0);

            // clamp movement beyond current tile
            if (currentTile != null)
            {
                if (walkingDirection == RIGHT)
                {
                    transform.position = new Vector3(Mathf.Clamp(transform.position.x, 0f, currentTile.transform.position.x), transform.position.y, transform.position.z);

                }
                else if (walkingDirection == LEFT)
                {
                    transform.position = new Vector3(Mathf.Clamp(transform.position.x, currentTile.transform.position.x, 10000F), transform.position.y, transform.position.z);
                }
            }

            rigidbody2D.velocity = Vector2.zero;
            paused = true;
        }
        // if enemy and player are on roughly same x axis, pause enemy
        else if (MLib.FloatEqual(transform.position.x, target.position.x, xAxisOffset))
        {
            rigidbody2D.velocity = Vector2.zero;
            paused = true;
        }
        else
        {
            paused = false;
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

    void OnPlayerDead(string causeOfDeath, Collider2D coll, int directionHit)
    {
        // causes enemy to do a victory dance
        xAxisOffset = .005f;
        CancelInvoke();
    }

    void OnDisable()
    {
        CancelInvoke();
    }

    void OnEnable()
    {
        Messenger.AddListener<string, Collider2D, int>( "player dead", OnPlayerDead);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener<string, Collider2D, int>( "player dead", OnPlayerDead);
    }
}
