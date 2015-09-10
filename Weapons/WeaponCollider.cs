using UnityEngine;
using System.Collections;
using Matcha.Lib;

[RequireComponent(typeof(BoxCollider2D))]


public class WeaponCollider : CacheBehaviour
{
    void Start()
    {
        // only enable WeaponCollider during attacks
        collider2D.enabled = false;
    }

	void OnTriggerEnter2D(Collider2D coll) {}

	void OnTriggerStay2D(Collider2D coll) {}

	void OnTriggerExit2D(Collider2D coll) {}
}