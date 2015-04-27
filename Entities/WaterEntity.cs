using UnityEngine;
using System.Collections;

public class WaterEntity : Entity
{
    private BoxCollider2D thisCollider;

    void Start()
    {
        thisCollider = GetComponent<BoxCollider2D>();
    }

	override public void OnBodyCollisionEnter()
	{
        collidedWithBody = true;

		if (!player.Dead)
			Messenger.Broadcast<string, Collider2D>("player dead", "drowned", thisCollider);
		else
			Messenger.Broadcast<string, Collider2D>("player drowned", "drowned", thisCollider);
	}

	override public void OnBodyCollisionStay() {}

	override public void OnBodyCollisionExit() {}

    override public void OnWeaponCollisionEnter() {}

    override public void OnWeaponCollisionStay() {}

    override public void OnWeaponCollisionExit() {}
}
