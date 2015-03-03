using UnityEngine;
using System.Collections;
using Matcha.Lib;

[RequireComponent(typeof(BoxCollider2D))]


public class BodyCollider : CacheBehaviour
{
	private _PlayerData player;
	private PlayerState state;
	private bool colliderDisabled;

	void Start()
	{
		MLib2D.IgnoreLayerCollisionWith(gameObject, "One-Way Platform", true);
		MLib2D.IgnoreLayerCollisionWith(gameObject, "Platform", true);
		player = GameObject.Find("_PlayerData").GetComponent<_PlayerData>();
		state = transform.parent.GetComponent<PlayerState>();
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

				case "Water":
					OnWaterCollisionEnter(coll);
					break;

				case "Wall":
					OnWallCollisionEnter(coll);
					break;

				case "SAVE":
					player.data.Save();
					break;

				case "LOAD":
				{
					player.data.Load();
					Debug.Log("HP = " + player.data.HP);
				}
					break;

			}
		}
	}

	void OnTriggerStay2D(Collider2D coll)
	{
		if (!colliderDisabled)
		{
			switch (coll.tag)
			{
				case "Wall":
					OnWallCollisionStay(coll);
					break;
			}
		}
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

				case "Water":
					OnWaterCollisionExit(coll);
					break;

				case "Wall":
					OnWallCollisionExit(coll);
					break;
			}
		}
	}


	// prize collision handlers
	private void OnPrizeCollisionEnter(Collider2D coll)
	{
		PickupEntity entity = GetPickupEntity(coll);

		if (!entity.AlreadyCollidedWithBody() && !state.Dead)
		{
			entity.SetCollidedWithBody(true);
			Messenger.Broadcast<int>("prize collected", entity.Worth());
			entity.ReactToCollision();
		}
	}


	// level-up collision handlers
	private void OnLevelUpCollisionEnter(Collider2D coll)
	{
		PickupEntity entity = GetPickupEntity(coll);

		if (!entity.AlreadyCollidedWithBody() && !state.Dead)
		{
			entity.SetCollidedWithBody(true);
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

		if (!entity.AlreadyCollidedWithBody() && !state.Dead)
		{
			entity.SetCollidedWithBody(true);

			// include state.Dead argument, because if player is already dead when colliding with this entity, there 
			// are some actions we don't want to take, such as reducing lives by 1, or restarting level a second time
		    Messenger.Broadcast<string, bool, Collider2D>("player dead", "StruckDown", state.Dead, coll);

		    state.Dead = true;
		}
	}

	private void OnEnemyCollisionExit(Collider2D coll)
	{
		CreatureEntity entity = GetCreatureEntity(coll);
		entity.SetCollidedWithBody(false);
	}


	// water collider handlers
	private void OnWaterCollisionEnter(Collider2D coll)
	{
		WaterEntity entity = GetWaterEntity(coll);

		if (!entity.AlreadyCollidedWithBody())
		{
			entity.SetCollidedWithBody(true);
			
			// include state.Dead argument, because if player is already dead when colliding with this entity, there 
			// are some actions we don't want to take, such as reducing lives by 1, or restarting level a second time
		    Messenger.Broadcast<string, bool, Collider2D>("player dead", "Drowned", state.Dead, coll);

			state.Dead = true;
		}
	}

	private void OnWaterCollisionExit(Collider2D coll)
	{
		WaterEntity entity = GetWaterEntity(coll);
		entity.SetCollidedWithBody(false);
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