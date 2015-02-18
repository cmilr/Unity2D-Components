using UnityEngine;
using System.Collections;
using Rewired;
using Matcha.Lib;

[RequireComponent(typeof(CharacterController2D))]
[RequireComponent(typeof(RunAndJump))]


public class DeathHandler : CacheBehaviour
{
	public float gravity = -35f;                        // set gravity for player
	public float runSpeed = 8f;                         // set player's run speed
	public float groundDamping = 20f;                   // how fast do we change direction? higher means faster
	public float inAirDamping = 5f;                     // how fast do we change direction mid-air?
	public float jumpHeight = 2.6f;                     // player's jump height
	public float maxFallingSpeed = 100f;                // max falling speed, for throttling falls, etc
	public float maxRisingSpeed = 2f;                   // max rising speed, for throttling player on moving platforms, etc
	
	private bool facingRight;
	private string deathAnimation = "StruckDown";

	private RaycastHit2D lastControllerColliderHit;
	private Vector3 velocity;
	private CharacterController2D controller;
	private float normalizedHorizontalSpeed;

	void Start()
	{
		base.CacheComponents();
		this.enabled = false;
		controller = GetComponent<CharacterController2D>();
	}

	void LateUpdate()
	{
		animator.Play(Animator.StringToHash(deathAnimation));

		velocity = controller.velocity;
		var smoothedMovementFactor = controller.isGrounded ? groundDamping : inAirDamping;
		velocity.x = Mathf.Lerp(velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor);
		velocity.y += gravity * Time.deltaTime;
		velocity.y = Mathf.Clamp(velocity.y, -maxFallingSpeed, maxFallingSpeed);
		controller.move(velocity * Time.deltaTime);
	}

	public void Enable()
	{
		this.enabled = true;
	}

	public void KillPlayer(string causeOfDeath, bool playerFacing)
	{
		facingRight = playerFacing;
		deathAnimation = causeOfDeath;

		switch (causeOfDeath)
		{
		case "StruckDown":
			StartCoroutine("Repulse");
			break;

		case "Drowned":
			break;

		default:
			StartCoroutine("Repulse");
			break;
		}
	}

	IEnumerator Repulse() 
	{
		if (facingRight)
			{
				normalizedHorizontalSpeed = -1;
			}
			else 
			{
				normalizedHorizontalSpeed = 1;
			}

	    for (float f = 1f; f >= 0; f -= 0.1f) 
	    {
	    	if (facingRight)
			{
				normalizedHorizontalSpeed += .1f;
			}
			else 
			{
				normalizedHorizontalSpeed -= .1f;
			}

	        yield return new WaitForSeconds(.1f);
	    }
	}
}