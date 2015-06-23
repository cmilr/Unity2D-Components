using Matcha.Game.Colors;   // testing only
using UnityEngine;
using System;
using System.Collections;
using Rotorz.Tile;
using Matcha.Extensions;

public class EnemyAI : CacheBehaviour {

    public bool test;
    public GameObject[] targets;    // testing only

    public float attackInterval    = 1f;
    public float chanceOfAttack    = 40f;
    public float attackWhenInRange = 20f;

    private TileSystem tileSystem;
    private TileData tile;
    private ProjectileManager projectile;
    private Weapon weapon;
    private int tileConvertedX;
    private int tileConvertedY;
    private Transform target;
    private bool moving;
    private bool stopped;

	void Start()
    {
        projectile = GetComponent<ProjectileManager>();
        weapon     = GetComponentInChildren<Weapon>();
        target     = GameObject.Find(PLAYER).transform;
        tileSystem = GameObject.Find(TILE_MAP).GetComponent<TileSystem>();
	}

    void OnBecameVisible()
    {
        InvokeRepeating("LookAtTarget", 1f, .3f);
        InvokeRepeating("Wander", 1f, .2f);

        if (test) {
            StartCoroutine(LobCompTest());
        } else {
            InvokeRepeating("AttackRandomly", 2f, attackInterval);
        }
    }

    void OnBecameInvisible()
    {
        CancelInvoke("LookAtTarget");

        if (!test)
            CancelInvoke("AttackRandomly");
    }

    void LookAtTarget()
    {
        int direction = (target.position.x > transform.position.x) ? RIGHT : LEFT;
        transform.localScale = new Vector3((float)direction, transform.localScale.y, transform.localScale.z);
    }

    void Wander()
    {
        if (!moving && !stopped)
        {
            rigidbody2D.velocity = transform.right * 2f * -1f;
            moving = true;
        }
        else if (stopped)
        {
            rigidbody2D.velocity = Vector2.zero;
        }


        GameObject obj = transform.GetTileBelowLeft(tileSystem);
        if (obj != null)
        {
            Debug.Log("TILE DETECTED: " + obj);
        }
        else
        {
            stopped = true;
        }
    }

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

    void OnDisable()
    {
        CancelInvoke();
    }








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
