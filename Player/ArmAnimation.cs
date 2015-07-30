using UnityEngine;
using System.Collections;

public class ArmAnimation : AnimationBehaviour, IPlayerAnimation {

    private string idleAnimation;
    private string runAnimation;
    private string jumpAnimation;
    private string swingAnimation;
    private string hurlAnimation;
    private IPlayerStateFullAccess state;

    void Start ()
    {
        state = GameObject.Find(PLAYER).GetComponent<IPlayerStateFullAccess>();
        SetAnimations(state.Character);
    }

    void SetAnimations(string character)
    {
        // uses string literals over concatenation in order to reduce GC calls
        if (character == "LAURA")
        {
            idleAnimation  = "LAURA_ARM_Idle";
            runAnimation   = "LAURA_ARM_Run";
            jumpAnimation  = "LAURA_ARM_Jump";
            swingAnimation = "LAURA_ARM_Swing";
            hurlAnimation  = "LAURA_ARM_Hurl";
            }
            else
            {
            idleAnimation  = "MAC_ARM_Idle";
            runAnimation   = "MAC_ARM_Run";
            jumpAnimation  = "MAC_ARM_Jump";
            swingAnimation = "MAC_ARM_Swing";
            hurlAnimation  = "Mac_ARM_Hurl";
        }
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

    public void PlayHurlAnimation(float xOffset, float yOffset)
    {
        animator.speed = HURL_SPEED;
        animator.Play(Animator.StringToHash(hurlAnimation));
        OffsetAnimation(xOffset, yOffset);
    }

    void OnPlayerDead(string methodOfDeath, Collider2D coll, int hitFrom)
    {
        spriteRenderer.enabled = false;
    }

    void OnEnable()
    {
        Messenger.AddListener<string, Collider2D, int>("player dead", OnPlayerDead);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener<string, Collider2D, int>( "player dead", OnPlayerDead);
    }
}