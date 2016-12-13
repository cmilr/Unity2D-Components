using UnityEngine;
using UnityEngine.Assertions;

public class AnimationState : BaseBehaviour
{
	[HideInInspector]
	public bool moving;
	[HideInInspector]
	public bool airborne;
	[HideInInspector]
	public bool attacking;
	Animator_Base playerAnimator;
	Animator_Weapon weaponAnimator;

	void Start()
	{
		playerAnimator = GetComponent<Animator_Base>();
		Assert.IsNotNull(playerAnimator);

		weaponAnimator = GetComponent<Animator_Weapon>();
		Assert.IsNotNull(weaponAnimator);
	}

	void Update()
	{
		// IDLE STATE
		if (!moving && !airborne && !attacking)
		{
			playerAnimator.PlayIdleAnimation();
			weaponAnimator.PlayIdleAnimation();
		}
		// ATTACKING STATE
		else if (!moving && !airborne && attacking)
		{
			playerAnimator.PlayAttackAnimation();
			weaponAnimator.PlayAttackAnimation();
		}
		// RUNNING STATE
		else if (moving && !airborne && !attacking)
		{
			playerAnimator.PlayRunAnimation();
			weaponAnimator.PlayRunAnimation();
		}
		// RUNNING ATTACK STATE
		else if (moving && !airborne && attacking)
		{
			playerAnimator.PlayRunAttackAnimation();
			weaponAnimator.PlayRunAttackAnimation();
		}
		// JUMPING STATE
		else if (airborne && !attacking)
		{
			playerAnimator.PlayJumpAnimation();
			weaponAnimator.PlayJumpAnimation();
		}
		// JUMPING ATTACK STATE
		else if (airborne && attacking)
		{
			playerAnimator.PlayJumpAttackAnimation();
			weaponAnimator.PlayJumpAttackAnimation();
		}
	}
}
