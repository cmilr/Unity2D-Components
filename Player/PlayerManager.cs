using Matcha.Dreadful;
using UnityEngine;

public class PlayerManager : CacheBehaviour
{
	private PlayerData player;
	private GameManager game;

	void Start()
	{
		player = GameObject.Find(_DATA).GetComponent<PlayerData>();
		game 	 = GameObject.Find(GAME_MANAGER).GetComponent<GameManager>();

		Init();
	}

	void Init()
	{
		EventKit.Broadcast<int>("init lvl", player.LVL);
		EventKit.Broadcast<int>("init hp", player.HP);
		EventKit.Broadcast<int>("init ac", player.AC);
		EventKit.Broadcast<int>("init xp", player.XP);
		EventKit.Broadcast<GameObject, GameObject, GameObject>
			("init weapons", player.equippedWeapon, player.leftWeapon, player.rightWeapon);
	}

	public void TakesHit(Hit hit)
	{
		// calculate damage
		player.HP -= (int)(hit.weapon.damage * game.dDamageMod);

		// produce effects
		// params for ShakeCamera = duration, strength, vibrato, randomness
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
		// calculate damage
		player.HP -= (int)(enemy.touchDamage * game.dDamageMod);

		// produce effects
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
}
