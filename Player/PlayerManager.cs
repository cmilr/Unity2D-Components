using DG.Tweening;
using Matcha.Dreadful;
using System.Collections;
using UnityEngine;

public class PlayerManager : CacheBehaviour
{
	private _PlayerData player;

	void Start()
	{
		player = GameObject.Find(_PLAYER_DATA).GetComponent<_PlayerData>();

		Init();
	}

	void Init()
	{
		Evnt.Broadcast<int>("init lvl", player.LVL);
		Evnt.Broadcast<int>("init hp", player.HP);
		Evnt.Broadcast<int>("init ac", player.AC);
		Evnt.Broadcast<int>("init xp", player.XP);
		Evnt.Broadcast<GameObject, GameObject, GameObject>
			("init weapons", player.equippedWeapon, player.leftWeapon, player.rightWeapon);
		Evnt.Broadcast<Transform>("player placed", transform);
	}

	public void TakesHit(string weaponType, Weapon weapon, Collider2D coll, int hitFrom)
	{
		// calculate damage
		player.HP -= (int)(weapon.damage * DIFFICULTY_DAMAGE_MODIFIER);

		// produce effects
		// params for ShakeCamera = duration, strength, vibrato, randomness
		Evnt.Broadcast<float, float, int, float>("shake camera", .5f, .3f, 20, 5f);
		Evnt.Broadcast<int>("reduce hp", player.HP);

		// float xDistance = hitFrom == LEFT ? 2f : -2f;
		// transform.DOJump(new Vector3(transform.position.x + xDistance, transform.position.y, transform.position.z), .2f, 1, .5f, false);

		if (hitFrom == RIGHT)
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
			Evnt.Broadcast<string, Collider2D, int>("player dead", "projectile", coll, hitFrom);
		}
	}

	public void TouchesEnemy(string weaponType, CreatureEntity enemy, Collider2D coll, int hitFrom)
	{
		// calculate damage
		player.HP -= (int)(enemy.touchDamage * DIFFICULTY_DAMAGE_MODIFIER);

		// produce effects
		Evnt.Broadcast<int>("reduce hp", player.HP);

		if (player.HP > 0)
		{
			MFX.FadeToColorAndBack(spriteRenderer, MCLR.bloodRed, 0f, .2f);
		}
		else
		{
			Evnt.Broadcast<string, Collider2D, int>("player dead", "struckdown", coll, hitFrom);
		}
	}

	void OnPlayerHit(string weaponType, Collider2D coll, int hitFrom)
	{
		// Evnt.Broadcast<string, Collider2D, int>("player dead", "projectile", coll, hitFrom);
	}

	void OnPrizeCollected(int worth)
	{
	}

	void OnLevelCompleted(bool status)
	{
	}

	void OnEnable()
	{
		Evnt.Subscribe<string, Collider2D, int>("player hit", OnPlayerHit);
		Evnt.Subscribe<int>("prize collected", OnPrizeCollected);
		Evnt.Subscribe<bool>("level completed", OnLevelCompleted);
	}

	void OnDestroy()
	{
		Evnt.Unsubscribe<string, Collider2D, int>("player hit", OnPlayerHit);
		Evnt.Unsubscribe<int>("prize collected", OnPrizeCollected);
		Evnt.Unsubscribe<bool>("level completed", OnLevelCompleted);
	}
}
