using UnityEngine;
using System.Collections;

public class WeaponPiece : CacheBehaviour {

    [HideInInspector]
    public string idleAnimation;
    [HideInInspector]
    public string runAnimation;
    [HideInInspector]
    public string jumpAnimation;
    [HideInInspector]
    public string swingAnimation;

    private Material material;

    void Start ()
    {
        SetAnimations();
    }

    void SetAnimations()
    {
        idleAnimation = name + "_Idle";
        runAnimation = name + "_Run";
        jumpAnimation = name + "_Jump";
        swingAnimation = name + "_Swing";
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