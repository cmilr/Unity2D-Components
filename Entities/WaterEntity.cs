using UnityEngine;
using System.Collections;

public class WaterEntity : Entity 
{
	override public void OnBodyCollisionEnter()
	{
		if (!playerDead)
			Messenger.Broadcast<string, Collider2D>("player dead", "drowned", GetComponent<BoxCollider2D>());
		else
			Messenger.Broadcast<string, Collider2D>("player drowned", "drowned", GetComponent<BoxCollider2D>());

	}

	override public void OnBodyCollisionStay() {}

	override public void OnBodyCollisionExit() {}
}
