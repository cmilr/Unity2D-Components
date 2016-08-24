using Matcha.Dreadful;
using UnityEngine;

public class PlayerManager : CacheBehaviour
{
	private int diffDamageModifier;
	private PlayerData player;

	void Start()
	{
		player = GameObject.Find(_DATA).GetComponent<PlayerData>();
		Init();
	}

	void Init()
	{
		EventKit.Broadcast<int>("init lvl", player.LVL);
		EventKit.Broadcast<int>("init hp", player.HP);
		EventKit.Broadcast<int>("init ac", player.AC);
		EventKit.Broadcast<int>("init xp", player.XP);
<<<<<<< HEAD
		EventKit.Broadcast<GameObject, GameObject, GameObject>
			("init weapons", player.equippedWeapon, player.leftWeapon, player.rightWeapon);
=======
		EventKit.Broadcast<GameObject, GameObject, GameObject>("init weapons",
					player.equippedWeapon, player.leftWeapon, player.rightWeapon);
>>>>>>> 6fa29b194fdad24bff4588056e6116fd14b7a700
	}

	public void TakesHit(Hit hit)
	{
<<<<<<< HEAD
		// calculate damage
		player.HP -= (int)(weapon.damage * DIFFICULTY_DAMAGE_MODIFIER);

		// produce effects
		// params for ShakeCamera = duration, strength, vibrato, randomness
=======
		player.HP -= (int)(hit.weapon.damage * diffDamageModifier);

		//params for ShakeCamera = duration, strength, vibrato, randomness
>>>>>>> 6fa29b194fdad24bff4588056e6116fd14b7a700
		EventKit.Broadcast<float, float, int, float>("shake camera", .5f, .3f, 20, 5f);
		EventKit.Broadcast<int>("reduce hp", player.HP);

		if (hit.hitFrom == RIGHT)
		{
			BroadcastMessage("RepulseToLeft", 5.0F);
		}
		else
		{
			BroadcastMessage("RepulseToRight", 5.0F);
		}

		if (player.HP > 0)
		{
			MFX.FadeToColorAndBack(spriteRenderer, MCLR.bloodRed, 0f, .2f);
		}
		else
		{
<<<<<<< HEAD
			EventKit.Broadcast<string, Collider2D, int>("player dead", "projectile", coll, hitFrom);
=======
			EventKit.Broadcast<int, Weapon.WeaponType>("player dead", hit.hitFrom, hit.weaponType);
>>>>>>> 6fa29b194fdad24bff4588056e6116fd14b7a700
		}
	}

	public void TouchesEnemy(string weaponType, CreatureEntity enemy, Collider2D coll, int hitFrom)
	{
		player.HP -= (int)(enemy.touchDamage * diffDamageModifier);

<<<<<<< HEAD
		// produce effects
=======
>>>>>>> 6fa29b194fdad24bff4588056e6116fd14b7a700
		EventKit.Broadcast<int>("reduce hp", player.HP);

		if (player.HP > 0)
		{
			MFX.FadeToColorAndBack(spriteRenderer, MCLR.bloodRed, 0f, .2f);
		}
		else
		{
<<<<<<< HEAD
			EventKit.Broadcast<string, Collider2D, int>("player dead", "struckdown", coll, hitFrom);
=======
			EventKit.Broadcast<int, Weapon.WeaponType>("player dead", hitFrom, Weapon.WeaponType.Struckdown);
>>>>>>> 6fa29b194fdad24bff4588056e6116fd14b7a700
		}
	}

	void OnSetDiffDamageModifier(int modifier)
	{
		diffDamageModifier = modifier;
	}

	void OnEnable()
	{
		EventKit.Subscribe<int>("set difficulty damage modifier", OnSetDiffDamageModifier);
	}

	void OnDestroy()
	{
		EventKit.Unsubscribe<int>("set difficulty damage modifier", OnSetDiffDamageModifier);
	}
}
