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
	AnimationHandler anim;

	void Awake()
	{
		anim = GetComponent<AnimationHandler>();
		Assert.IsNotNull(anim);
	}

	void Update()
	{
		// IDLE STATE
		if (!moving && !airborne && !attacking)
		{
			anim.PlayIdleAnimation();
		}
		// ATTACKING STATE
		else if (!moving && !airborne && attacking)
		{
			anim.PlayAttackAnimation();
		}
		// RUNNING STATE
		else if (moving && !airborne && !attacking)
		{
			anim.PlayRunAnimation();
		}
		// RUNNING ATTACK STATE
		else if (moving && !airborne && attacking)
		{
			anim.PlayRunAttackAnimation();
		}
		// JUMPING STATE
		else if (airborne && !attacking)
		{
			anim.PlayJumpAnimation();
		}
		// JUMPING ATTACK STATE
		else if (airborne && attacking)
		{
			anim.PlayJumpAttackAnimation();
		}
	}
}
