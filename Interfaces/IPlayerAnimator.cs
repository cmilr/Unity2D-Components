
public interface IPlayerAnimator
{
	void PlayIdleAnimation();
	void PlayRunAnimation();
	void PlayJumpAnimation();
	void PlayAttackAnimation();
	void PlayRunAttackAnimation();
	void PlayJumpAttackAnimation();
	bool ExitTimeReached();
}