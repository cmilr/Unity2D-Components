using Matcha.Unity;
using UnityEngine;
using UnityEngine.Assertions;

public class AttackAI : BaseBehaviour
{
	public enum Style { Invalid, RandomProjectile };
	public Style style;
	public float chanceOfAttack      = 40f;
	public float attackWhenInRange   = 30f;
	public bool attackPaused;
	
	private float attackInterval;
	private bool levelLoading;
	private bool dead;
	private ProjectileManager projectile;
	private Weapon weapon;
	private Transform target;
	private new Transform transform;

	void Awake()
	{
		transform = GetComponent<Transform>();
		Assert.IsNotNull(transform);

		projectile = GetComponent<ProjectileManager>();
		Assert.IsNotNull(projectile);

		Assert.IsFalse(style == Style.Invalid,
	   		("Invalid attack style @ " + gameObject));
	}
	
	void Start()
	{		
		weapon = GetComponentInChildren<Weapon>();
		Assert.IsNotNull(weapon);
		
		target = GameObject.Find(PLAYER).transform;
		Assert.IsNotNull(target);
		
		attackInterval = Rand.Range(1.5f, 2.5f);
	}

	//master controller
	void OnBecameVisible()
	{
		if (!attackPaused && !dead)
		{
			switch (style)
			{
				case Style.RandomProjectile:
					InvokeRepeating("AttackRandomly", 2f, attackInterval);
					break;
				default:
					Assert.IsTrue(false, ("Attack style missing from switch @ " + gameObject));
					break;
			}

		}
	}

	void AttackRandomly()
	{
		if (!debug_AttackDisabled)
		{
			if (!attackPaused && !levelLoading && !dead)
			{
				float distance = Vector3.Distance(target.position, transform.position);

				if (distance <= attackWhenInRange)
				{
					if (Rand.Range(1, 100) <= chanceOfAttack)
					{
						//only attack if creature is facing the direction of target
						if ((target.position.x > transform.position.x && transform.localScale.x.FloatEquals(1f)) ||
								(target.position.x < transform.position.x && transform.localScale.x.FloatEquals(-1f)))
						{
							projectile.FireAtTarget(weapon, target);
						}
					}
				}
			}
		}
	}

	void RotateTowardsTarget()
	{
		if (!attackPaused && !dead)
		{
			Vector3 vel = GetForceFrom(transform.position, target.position);
			float angle = Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg;
			transform.eulerAngles = new Vector3(0, 0, angle);
		}
	}

	static Vector2 GetForceFrom(Vector3 fromPos, Vector3 toPos)
	{
		const float power = 1;

		return (new Vector2(toPos.x, toPos.y) - new Vector2(fromPos.x, fromPos.y)) * power;
	}

	void CreatureDead()
	{
		dead = true;
		attackPaused = true;
		enabled = false;
	}

	void OnDisable()
	{
		CancelInvoke();
	}

	void OnLevelLoading(bool status)
	{
		//pause attacks and other activities while level loads
		levelLoading = true;

		StartCoroutine(Timer.Start(PAUSE_ENEMIES_WHILE_LVL_LOADS, false, () =>
		{
			levelLoading = false;
		}));
	}

	void OnEnable()
	{
		EventKit.Subscribe<bool>("level loading", OnLevelLoading);
	}

	void OnDestroy()
	{
		EventKit.Unsubscribe<bool>("level loading", OnLevelLoading);
	}



	//TARGET TESTING SUITE
	//####################

	//public bool test;
	//public GameObject[] targets;
	////testing only

	//IEnumerator LobCompTest()
	//{
	//	int i = 0;
	//	int j = i - 1;

	//	while (true)
	//	{
	//		if (i >= targets.Length) {
	//			i = 0;
	//		}

	//		if (j >= targets.Length) {
	//			j = 0;
	//		}

	//		if (j < 0) {
	//			j = 10;
	//		}

	//		targets[i].GetComponent<SpriteRenderer>().material.SetColor("_Color", MCLR.orange);
	//		targets[j].GetComponent<SpriteRenderer>().material.SetColor("_Color", MCLR.white);
	//		projectile.FireAtTarget(weapon, targets[i].transform);
	//		i++;
	//		j++;
	//		yield return new WaitForSeconds(2);
	//	}
	//}
}
