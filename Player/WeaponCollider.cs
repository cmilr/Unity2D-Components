using UnityEngine;
using System.Collections;
using Matcha.Lib;

[RequireComponent(typeof(BoxCollider2D))]


public class WeaponCollider : CacheBehaviour
{
	private PlayerState state;
	private bool colliderDisabled;
	

	void Start()
	{
		state = transform.parent.GetComponent<PlayerState>();

		MLib2D.IgnoreLayerCollisionWith(gameObject, "One-Way Platform", true);
		MLib2D.IgnoreLayerCollisionWith(gameObject, "Platform", true);
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (!colliderDisabled)
		{
			switch (coll.tag)
			{
				case "Prize":
					OnPrizeCollisionEnter(coll);
					break;

				case "Enemy":
					OnEnemyCollisionEnter(coll);
					break;

				case "LevelUp":
					OnLevelUpCollisionEnter(coll);
					break;
			}
		}
	}

	void OnTriggerStay2D(Collider2D coll)
	{
		// if (!colliderDisabled)
		// {
		// 	switch (coll.tag)
		// 	{
		// 		case "Wall":
		// 			OnWallCollisionStay(coll);
		// 			break;
		// 	}
		// }
	}

	void OnTriggerExit2D(Collider2D coll)
	{
		if (!colliderDisabled)
		{
			switch (coll.tag)
			{
				case "Enemy":
					OnEnemyCollisionExit(coll);
					break;
			}
		}
	}


	// prize collision handlers
	private void OnPrizeCollisionEnter(Collider2D coll)
	{
		PickupEntity entity = GetPickupEntity(coll);

		if (!entity.AlreadyCollidedWithWeapon() && !state.Dead)
		{
			entity.SetCollidedWithWeapon(true);
			Messenger.Broadcast<int>("prize collected", entity.Worth());
			entity.ReactToCollision();
		}
	}


	// level-up collision handlers
	private void OnLevelUpCollisionEnter(Collider2D coll)
	{
		PickupEntity entity = GetPickupEntity(coll);

		if (!entity.AlreadyCollidedWithWeapon() && !state.Dead)
		{
			entity.SetCollidedWithWeapon(true);
			Messenger.Broadcast<int>("prize collected", entity.Worth());
			entity.ReactToCollision();
		    Messenger.Broadcast<bool>("level completed", true);

		    colliderDisabled = true;
		}
	}


	// enemy collision handlers
	private void OnEnemyCollisionEnter(Collider2D coll)
	{
		CreatureEntity entity = GetCreatureEntity(coll);

		if (!entity.AlreadyCollidedWithWeapon() && !state.Dead)
		{
			entity.SetCollidedWithWeapon(true);
		    Messenger.Broadcast<string, Collider2D>("has died", "StruckDown", coll);
		}
	}

	private void OnEnemyCollisionExit(Collider2D coll)
	{
		CreatureEntity entity = GetCreatureEntity(coll);
		entity.SetCollidedWithWeapon(false);
	}


	// water collider handlers
	private void OnWaterCollisionEnter(Collider2D coll)
	{
		WaterEntity entity = GetWaterEntity(coll);

		if (!entity.AlreadyCollidedWithWeapon())
		{
			entity.SetCollidedWithWeapon(true);
		    Messenger.Broadcast<string, Collider2D>("has died", "Drowned", coll);
		}
	}

	private void OnWaterCollisionExit(Collider2D coll)
	{
		WaterEntity entity = GetWaterEntity(coll);
		entity.SetCollidedWithWeapon(false);
	}


	// wall collision handlers
	private void OnWallCollisionEnter(Collider2D coll)
	{
		Messenger.Broadcast<bool>("touching wall", true);
	}

	private void OnWallCollisionStay(Collider2D coll)
	{
		Messenger.Broadcast<bool>("touching wall", true);
	}

	private void OnWallCollisionExit(Collider2D coll)
	{
		Messenger.Broadcast<bool>("touching wall", false);
	}


	// get collider components
	private PickupEntity GetPickupEntity(Collider2D coll)
	{
		return coll.GetComponent<PickupEntity>() as PickupEntity;
	}

	private CreatureEntity GetCreatureEntity(Collider2D coll)
	{
		return coll.GetComponent<CreatureEntity>() as CreatureEntity;
	}

	private WaterEntity GetWaterEntity(Collider2D coll)
	{
		return coll.GetComponent<WaterEntity>() as WaterEntity;
	}
}