using UnityEngine;
using UnityEngine.Assertions;

public class Animator_Base : AnimatorBehaviour, IPlayerAnimator
{

	// A N I M A T I O N S
	// ~~~~~~~~~~~~~~~~~~~
	private readonly int idle 	= Animator.StringToHash("BASE_Idle");
	private readonly int run 	= Animator.StringToHash("BASE_Run");
	private readonly int jump 	= Animator.StringToHash("BASE_Jump");
	private readonly int attack = Animator.StringToHash("BASE_Attack");


	// T R A N S I T I O N S
	// ~~~~~~~~~~~~~~~~~~~~~
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


	// C L I P S   T H A T   H A V E   E X I T   T I M E S
	// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	public bool ExitTimeReached()
	{
		return true;
	}


	// C A L L   A N I M A T I O N S
	// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	public void PlayIdleAnimation()
	{
		if (ExitTimeReached())
		{
			anim.PlayClip(idle, IDLE_SPEED);

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
			anim.PlayClip(run, RUN_SPEED);

			currentlyPlaying = attack;
		}
	}

	public void PlayJumpAttackAnimation()
	{
		if (ExitTimeReached())
		{
			anim.PlayClip(jump, JUMP_SPEED);

			currentlyPlaying = attack;
		}
	}
}
