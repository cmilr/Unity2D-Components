using UnityEngine;
using System.Collections;

public class PhysicsCollider : CacheBehaviour {

	private BoxCollider2D physicsCollider;

	void Start () {
	
		physicsCollider = gameObject.GetComponent<BoxCollider2D>();
		SetColliderToNarrow();
	}

	void SetColliderToWide()
	{
		physicsCollider.size = new Vector3(1.4f, 1.2f, 2f);
	}

	void SetColliderToNarrow()
	{
		physicsCollider.size = new Vector3(.4f, 1.2f, 2f);
	}

	void OnEnable()
	{
		Messenger.AddListener<string, bool>( "player dead", OnPlayerDead);
	}

	void OnDestroy()
	{
		Messenger.RemoveListener<string, bool>( "player dead", OnPlayerDead);
	}

	void OnPlayerDead(string methodOfDeath, bool status)
	{
		SetColliderToWide();
	}
}
