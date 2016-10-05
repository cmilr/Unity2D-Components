using UnityEngine;
using UnityEngine.Assertions;

public class AnimationHandler : BaseBehaviour {

	private string idleAnimation     = "BASE_Idle";
	private string runAnimation      = "BASE_Run";
	private string jumpAnimation     = "BASE_Jump";
	private string attackAnimation   = "BASE_Attack";
	private string weaponIdleAnimation;
	private string weaponRunAnimation;
	private string weaponJumpAnimation;
	private string weaponAttackAnimation;
	private int currentAction;
	private Animator playerAnimator;
	private Animator weaponAnimator;
	private SpriteRenderer spriteRenderer;
	
	void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		Assert.IsNotNull(spriteRenderer);

		playerAnimator = GetComponent<Animator>();
		Assert.IsNotNull(playerAnimator);
	}
	
	void Start()
	{
		weaponAnimator = GameObject.Find("WeaponManager").GetComponent<Animator>();
		Assert.IsNotNull(weaponAnimator);
	}

	void NewWeaponEquipped(Weapon.Type weaponType)
	{
		switch (weaponType)
		{
			case Weapon.Type.Sword:
				weaponIdleAnimation = "SWORD_Idle";
				weaponRunAnimation = "SWORD_Run";
				weaponJumpAnimation = "SWORD_Jump";
				weaponAttackAnimation = "SWORD_Attack";
				break;
			case Weapon.Type.Axe:
				weaponIdleAnimation = "SWORD_Idle";
				weaponRunAnimation = "SWORD_Run";
				weaponJumpAnimation = "SWORD_Jump";
				weaponAttackAnimation = "SWORD_Attack";
				break;
			case Weapon.Type.Hammer:
				weaponIdleAnimation = "SWORD_Idle";
				weaponRunAnimation = "SWORD_Run";
				weaponJumpAnimation = "SWORD_Jump";
				weaponAttackAnimation = "SWORD_Attack";
				break;
		}
	}

	public void PlayIdleAnimation()
	{
		if (currentAction != IDLE)
		{
			playerAnimator.Play(Animator.StringToHash(idleAnimation));
			playerAnimator.SetFloat("VariablePlayerSpeed", IDLE_SPEED);
			weaponAnimator.Play(Animator.StringToHash(weaponIdleAnimation));
			weaponAnimator.SetFloat("VariableWeaponSpeed", IDLE_SPEED);
		}

		currentAction = IDLE;
	}
	
	//public void PlayRunAnimation()
	//{
	//	if (currentAction != RUN)
	//	{
	//		playerAnimator.Play(Animator.StringToHash(runAnimation), 0, 0f);
	//		// sync weapon animator time to player animator time.
	//		weaponAnimator.Play(Animator.StringToHash(weaponRunAnimation), 0, 0f);

	//		playerAnimator.SetFloat("VariablePlayerSpeed", RUN_SPEED);
	//		weaponAnimator.SetFloat("VariableWeaponSpeed", RUN_SPEED);
	//	}

	//	currentAction = RUN;
	//}

	public void PlayRunAnimation()
	{
		playerAnimator.Play(Animator.StringToHash(runAnimation));
		// sync weapon animator time to player animator time.
		weaponAnimator.Play(Animator.StringToHash(weaponRunAnimation), 0,
			playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);
		
		playerAnimator.SetFloat("VariablePlayerSpeed", RUN_SPEED);
		weaponAnimator.SetFloat("VariableWeaponSpeed", RUN_SPEED);

		currentAction = RUN;
	}

	public void PlayJumpAnimation()
	{
		if (currentAction != JUMP)
		{
			playerAnimator.Play(Animator.StringToHash(jumpAnimation));
			playerAnimator.SetFloat("VariablePlayerSpeed", JUMP_SPEED);
			weaponAnimator.Play(Animator.StringToHash(weaponJumpAnimation));
			weaponAnimator.SetFloat("VariableWeaponSpeed", JUMP_SPEED);
		}

		currentAction = JUMP;
	}

	public void PlayAttackAnimation()
	{
		if (currentAction != ATTACK)
		{
			playerAnimator.Play(Animator.StringToHash(attackAnimation));
			playerAnimator.SetFloat("VariablePlayerSpeed", ATTACK_SPEED);
			weaponAnimator.Play(Animator.StringToHash(weaponAttackAnimation));
			weaponAnimator.SetFloat("VariableWeaponSpeed", ATTACK_SPEED);
		}

		currentAction = ATTACK;
	}

	public void PlayRunAttackAnimation()
	{
		if (currentAction != RUN_ATTACK)
		{
			playerAnimator.Play(Animator.StringToHash(runAnimation));
			playerAnimator.SetFloat("VariablePlayerSpeed", RUN_SPEED);
			weaponAnimator.Play(Animator.StringToHash(weaponAttackAnimation));
			weaponAnimator.SetFloat("VariableWeaponSpeed", ATTACK_SPEED);
		}

		currentAction = RUN_ATTACK;
	}

	public void PlayJumpAttackAnimation()
	{
		if (currentAction != JUMP_ATTACK)
		{
			playerAnimator.Play(Animator.StringToHash(jumpAnimation));
			playerAnimator.SetFloat("VariablePlayerSpeed", JUMP_SPEED);
			weaponAnimator.Play(Animator.StringToHash(weaponAttackAnimation));
			weaponAnimator.SetFloat("VariableWeaponSpeed", ATTACK_SPEED);
		}

		currentAction = JUMP_ATTACK;
	}

	void OnPlayerDead(Hit incomingHit)
	{
		spriteRenderer.enabled = false;
	}

	void OnEnable()
	{
		EventKit.Subscribe<Hit>("player dead", OnPlayerDead);
	}

	void OnDestroy()
	{
		EventKit.Unsubscribe<Hit>("player dead", OnPlayerDead);
	}
}
