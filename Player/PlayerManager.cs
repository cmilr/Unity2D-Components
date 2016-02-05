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
		EventKit.Broadcast<GameObject, GameObject, GameObject>("init weapons",
					player.equippedWeapon, player.leftWeapon, player.rightWeapon);
	}

	public void TakesHit(Hit hit)
	{
		player.HP -= (int)(hit.weapon.damage * diffDamageModifier);

		//params for ShakeCamera = duration, strength, vibrato, randomness
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
			EventKit.Broadcast<int, Weapon.WeaponType>("player dead", hit.hitFrom, hit.weaponType);
		}
	}

	public void TouchesEnemy(string weaponType, CreatureEntity enemy, Collider2D coll, int hitFrom)
	{
		player.HP -= (int)(enemy.touchDamage * diffDamageModifier);

		EventKit.Broadcast<int>("reduce hp", player.HP);

		if (player.HP > 0)
		{
			MFX.FadeToColorAndBack(spriteRenderer, MCLR.bloodRed, 0f, .2f);
		}
		else
		{
			EventKit.Broadcast<int, Weapon.WeaponType>("player dead", hitFrom, Weapon.WeaponType.Struckdown);
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
