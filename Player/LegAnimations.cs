using UnityEngine;
using System.Collections;

public class LegAnimations : CacheBehaviour, IPlayerAnimation {

    private string idleAnimation;
    private string runAnimation;
    private string jumpAnimation;
    private string swingAnimation;
    private string name;
    private bool alreadyOffset;

    void Start ()
    {
        name = "LAURA";
        SetAnimations();
    }

    void SetAnimations()
    {
        idleAnimation = name + "_Idle_Legs";
        runAnimation = name + "_Run_Legs";
        jumpAnimation = name + "_Jump_Legs";
        swingAnimation = name + "_Swing_Legs";
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
        if (!alreadyOffset)
        {
            transform.position = new Vector3(transform.position.x, (transform.position.y + offset), transform.position.z);
        }

        alreadyOffset = true;
    }
}