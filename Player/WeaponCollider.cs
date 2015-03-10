using UnityEngine;
using System.Collections;
using Matcha.Lib;

[RequireComponent(typeof(BoxCollider2D))]


public class WeaponCollider : CacheBehaviour
{
	void Start()
	{
		MLib2D.IgnoreLayerCollisionWith(gameObject, "One-Way Platform", true);
		MLib2D.IgnoreLayerCollisionWith(gameObject, "Platform", true);
	}

	void OnTriggerEnter2D(Collider2D coll) {}

	void OnTriggerStay2D(Collider2D coll) {}

	void OnTriggerExit2D(Collider2D coll) {}

}