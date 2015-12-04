using UnityEngine;

public class WeaponPiece : CacheBehaviour
{
	[HideInInspector]
	public string idleAnimation;
	[HideInInspector]
	public string runAnimation;
	[HideInInspector]
	public string jumpAnimation;
	[HideInInspector]
	public string attackAnimation;

	private string weaponType;
	private Material material;

	void Start()
	{
		SetAnimations();
	}

	void SetAnimations()
	{
		idleAnimation   = name + "_Idle_";
		runAnimation    = name + "_Run_";
		jumpAnimation   = name + "_Jump_";
		attackAnimation = name + "_Attack_";
	}

	public void PlayIdleAnimation()
	{
		animator.speed = IDLE_SPEED;
		animator.Play(Animator.StringToHash(idleAnimation));
	}

	public void PlayRunAnimation()
	{
		animator.speed = RUN_SPEED;
		animator.Play(Animator.StringToHash(runAnimation));
	}

	public void PlayJumpAnimation()
	{
		animator.speed = JUMP_SPEED;
		animator.Play(Animator.StringToHash(jumpAnimation));
	}

	public void PlayAttackAnimation()
	{
		animator.speed = SWING_SPEED;
		animator.Play(Animator.StringToHash(attackAnimation));
	}

	void OnPlayerDead(string methodOfDeath, Collider2D coll, int hitFrom)
	{
		spriteRenderer.enabled = false;
	}

	void OnEnable()
	{
		Messenger.AddListener<string, Collider2D, int>("player dead", OnPlayerDead);
	}

	void OnDestroy()
	{
		Messenger.RemoveListener<string, Collider2D, int>("player dead", OnPlayerDead);
	}
}
