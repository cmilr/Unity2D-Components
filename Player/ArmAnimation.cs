using UnityEngine;
using System.Collections;

public class ArmAnimation : AnimationBehaviour, IPlayerAnimation {

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

    public void PlayIdleAnimation(float xOffset, float yOffset)
    {
        animator.speed = IDLE_SPEED;
        animator.Play(Animator.StringToHash(idleAnimation));
        OffsetAnimation(xOffset, yOffset);
    }

    public void PlayRunAnimation(float xOffset, float yOffset)
    {
        animator.speed = RUN_SPEED;
        animator.Play(Animator.StringToHash(runAnimation));
        OffsetAnimation(xOffset, yOffset);
    }

    public void PlayJumpAnimation(float xOffset, float yOffset)
    {
        animator.speed = JUMP_SPEED;
        animator.Play(Animator.StringToHash(jumpAnimation));
        OffsetAnimation(xOffset, yOffset);
    }

    public void PlaySwingAnimation(float xOffset, float yOffset)
    {
        animator.speed = SWING_SPEED;
        animator.Play(Animator.StringToHash(swingAnimation));
        OffsetAnimation(xOffset, yOffset);
    }

    void OnPlayerDead(string methodOfDeath, Collider2D coll)
    {
        spriteRenderer.enabled = false;
    }

    void OnEnable()
    {
        Messenger.AddListener<string, Collider2D>("player dead", OnPlayerDead);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener<string, Collider2D>( "player dead", OnPlayerDead);
    }
}