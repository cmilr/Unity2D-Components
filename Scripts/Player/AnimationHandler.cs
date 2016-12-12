using UnityEngine;
using UnityEngine.Assertions;

public class AnimationHandler : BaseBehaviour {

	private enum PlayerAction : byte { Idle, Run, Jump, Attack, RunAttack, JumpAttack }

	private const float IDLE_SPEED 			= 1f;
	private const float RUN_SPEED			= .5f;
	private const float JUMP_SPEED 			= 8f;
	private const float ATTACK_SPEED 		= 1.2f;
	private const float HURL_SPEED 			= 1f;

	private string idleAnimation     		= "BASE_Idle";
	private string runAnimation      		= "BASE_Run";
	private string jumpAnimation     		= "BASE_Jump";
	private string attackAnimation   		= "BASE_Attack";
	private string weaponIdleAnimation		= "SWORD_Idle";
	private string weaponRunAnimation		= "SWORD_Run";
	private string weaponJumpAnimation 		= "SWORD_Jump";
	private string weaponAttackAnimation	= "SWORD_Attack";
	private PlayerAction currentAction;
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
		if (currentAction != PlayerAction.Idle)
		{
			playerAnimator.Play(Animator.StringToHash(idleAnimation));
			playerAnimator.SetFloat("VariablePlayerSpeed", IDLE_SPEED);
			weaponAnimator.Play(Animator.StringToHash(weaponIdleAnimation));
			weaponAnimator.SetFloat("VariableWeaponSpeed", IDLE_SPEED);
		}

		currentAction = PlayerAction.Idle;
	}

	public void PlayRunAnimation()
	{
		playerAnimator.Play(Animator.StringToHash(runAnimation));
		// sync weapon animator time to player animator time.
		weaponAnimator.Play(Animator.StringToHash(weaponRunAnimation), 0,
			playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);
		
		playerAnimator.SetFloat("VariablePlayerSpeed", RUN_SPEED);
		weaponAnimator.SetFloat("VariableWeaponSpeed", RUN_SPEED);

		currentAction = PlayerAction.Run;
	}

	public void PlayJumpAnimation()
	{
		if (currentAction != PlayerAction.Jump)
		{
			playerAnimator.Play(Animator.StringToHash(jumpAnimation));
			playerAnimator.SetFloat("VariablePlayerSpeed", JUMP_SPEED);
			weaponAnimator.Play(Animator.StringToHash(weaponJumpAnimation));
			weaponAnimator.SetFloat("VariableWeaponSpeed", JUMP_SPEED);
		}

		currentAction = PlayerAction.Jump;
	}

	public void PlayAttackAnimation()
	{
		if (currentAction != PlayerAction.Attack)
		{
			playerAnimator.Play(Animator.StringToHash(attackAnimation));
			playerAnimator.SetFloat("VariablePlayerSpeed", ATTACK_SPEED);
			weaponAnimator.Play(Animator.StringToHash(weaponAttackAnimation));
			weaponAnimator.SetFloat("VariableWeaponSpeed", ATTACK_SPEED);
		}

		currentAction = PlayerAction.Attack;
	}

	public void PlayRunAttackAnimation()
	{
		if (currentAction != PlayerAction.RunAttack)
		{
			playerAnimator.Play(Animator.StringToHash(runAnimation));
			playerAnimator.SetFloat("VariablePlayerSpeed", RUN_SPEED);
			weaponAnimator.Play(Animator.StringToHash(weaponAttackAnimation));
			weaponAnimator.SetFloat("VariableWeaponSpeed", ATTACK_SPEED);
		}

		currentAction = PlayerAction.RunAttack;
	}

	public void PlayJumpAttackAnimation()
	{
		if (currentAction != PlayerAction.JumpAttack)
		{
			playerAnimator.Play(Animator.StringToHash(jumpAnimation));
			playerAnimator.SetFloat("VariablePlayerSpeed", JUMP_SPEED);
			weaponAnimator.Play(Animator.StringToHash(weaponAttackAnimation));
			weaponAnimator.SetFloat("VariableWeaponSpeed", ATTACK_SPEED);
		}

		currentAction = PlayerAction.JumpAttack;
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
