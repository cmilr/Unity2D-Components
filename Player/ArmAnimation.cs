using UnityEngine;
using System.Collections;

public class ArmAnimation : CacheBehaviour, IPlayerAnimation {

    private string idleAnimation;
    private string runAnimation;
    private string jumpAnimation;
    private string swingAnimation;

    void Start ()
    {
        SetAnimations();
    }

    void SetAnimations()
    {
        idleAnimation = character + "_ARM_Idle";
        runAnimation = character + "_ARM_Run";
        jumpAnimation = character + "_ARM_Jump";
        swingAnimation = character + "_ARM_Swing";
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

    public void OffsetX(float offset)
    {
        transform.localPosition = new Vector3(offset, 0, 0);
    }

    public void OffsetY(float offset)
    {
        transform.localPosition = new Vector3(0, offset, 0);
    }
}