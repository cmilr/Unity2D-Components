using UnityEngine;
using System.Collections;

public class WeaponHandler : CacheBehaviour {

    public string idleAnimation;
    public string runAnimation;
    public string jumpAnimation;

	void Start ()
    {
	   // spriteRenderer.color = new Color(0f, 0f, 0f, 1f); // Set to opaque black
       SetAnimations("SWORD");
	}

    void SetAnimations(string weapon)
    {
        idleAnimation = weapon + "_Idle";
        runAnimation = weapon + "_Run";
        jumpAnimation = weapon + "_Jump";
    }

    void OnPlayerDead(string methodOfDeath, Collider2D coll)
    {
        spriteRenderer.enabled = false;
    }

    void OnEnable()
    {
        Messenger.AddListener<string, Collider2D>("player dead", OnPlayerDead);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener<string, Collider2D>( "player dead", OnPlayerDead);
    }
}




