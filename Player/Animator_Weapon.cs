using UnityEngine;
using UnityEngine.Assertions;

public class Animator_Weapon : AnimatorBehaviour, IPlayerAnimator
{
	// A N I M A T I O N S
	// ~~~~~~~~~~~~~~~~~~~
	private int idle 	= Animator.StringToHash("SWORD_Idle");
	private int run 	= Animator.StringToHash("SWORD_Run");
	private int jump 	= Animator.StringToHash("SWORD_Jump");
	private int attack 	= Animator.StringToHash("SWORD_Attack2");

	// T R A N S I T I O N S
	// ~~~~~~~~~~~~~~~~~~~~~
	private int transition_AttackToIdle = Animator.StringToHash("SWORD_Attack_Into_Idle");


	#region Cache References
	private int clip;
	private int currentlyPlaying;

	[SerializeField]
	private Animator anim;

	void Awake()
	{
		Assert.IsNotNull(anim, ("Missing reference to Animator. Connect in inspector @ " + gameObject));
	}
	#endregion


	// S E T U P   W E A P O N   A N I M A T I O N S
	// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	void NewWeaponEquipped(Weapon.Type weaponType)
	{
		switch (weaponType)
		{
			case Weapon.Type.Sword:
				idle 	= Animator.StringToHash("SWORD_Idle");
				run 	= Animator.StringToHash("SWORD_Run");
				jump 	= Animator.StringToHash("SWORD_Jump");
				attack 	= Animator.StringToHash("SWORD_Attack2");
				break;
			case Weapon.Type.Axe:
				idle 	= Animator.StringToHash("SWORD_Idle");
				run 	= Animator.StringToHash("SWORD_Run");
				jump 	= Animator.StringToHash("SWORD_Jump");
				attack 	= Animator.StringToHash("SWORD_Attack2");
				break;
			case Weapon.Type.Hammer:
				idle 	= Animator.StringToHash("SWORD_Idle");
				run 	= Animator.StringToHash("SWORD_Run");
				jump 	= Animator.StringToHash("SWORD_Jump");
				attack 	= Animator.StringToHash("SWORD_Attack2");
				break;
		}
	}


	// C L I P S   T H A T   H A V E   E X I T   T I M E S
	// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	public bool ExitTimeReached()
	{
		clip = anim.GetNameHash();

		// add all animations that require at least one full loop
		if (clip == attack 						&& anim.IsMidLoop()) { return false; }
		if (clip == transition_AttackToIdle 	&& anim.IsMidLoop()) { return false; }

		return true;
	}


	// C A L L   A N I M A T I O N S
	// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	public void PlayIdleAnimation()
	{
		if (ExitTimeReached())						// if current clip has reached its exit time, choose next clip to play.
		{
			if (currentlyPlaying == attack)			// if a transition is needed, detect it here and play proper clip.
			{
				anim.PlayClip(transition_AttackToIdle, IDLE_SPEED);
			}
			else  									// otherwise play regular idle clip.
			{
				anim.PlayClip(idle, IDLE_SPEED);
			}

			currentlyPlaying = idle;
		}
	}

	public void PlayRunAnimation()
	{
		if (ExitTimeReached())
		{
			anim.PlayClip(run, RUN_SPEED);

			currentlyPlaying = run;
		}
	}

	public void PlayJumpAnimation()
	{
		if (ExitTimeReached())
		{
			anim.PlayClip(jump, JUMP_SPEED);

			currentlyPlaying = jump;
		}
	}

	public void PlayAttackAnimation()
	{
		if (ExitTimeReached())
		{
			anim.PlayClip(attack, ATTACK_SPEED);

			currentlyPlaying = attack;
		}
	}

	public void PlayRunAttackAnimation()
	{
		if (ExitTimeReached())
		{
			anim.PlayClip(attack, ATTACK_SPEED);

			currentlyPlaying = attack;
		}
	}

	public void PlayJumpAttackAnimation()
	{
		if (ExitTimeReached())
		{
			anim.PlayClip(attack, ATTACK_SPEED);

			currentlyPlaying = attack;
		}
	}
}