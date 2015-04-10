using UnityEngine;
using System.Collections;

public class ArmAnimation : CacheBehaviour, IPlayerAnimation {

    private string idleAnimation;
    private string runAnimation;
    private string jumpAnimation;
    private string swingAnimation;
    private string playerName = "LAURA";

    void Start ()
    {
        SetAnimations();
    }

    void SetAnimations()
    {
        idleAnimation = playerName + "_ARM_Idle";
        runAnimation = playerName + "_ARM_Run";
        jumpAnimation = playerName + "_ARM_Jump";
        swingAnimation = playerName + "_ARM_Swing";
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

    public void PlaySwingAnimation()
    {
        animator.speed = SWING_SPEED;
        animator.Play(Animator.StringToHash(swingAnimation));
    }

    public void OffsetAnimationBy(float offset)
    {

    }
}