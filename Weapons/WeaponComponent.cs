using UnityEngine;
using System.Collections;
using Matcha.Game.Colors;

public class WeaponComponent : CacheBehaviour {

    public string idleAnimation;
    public string runAnimation;
    public string jumpAnimation;
    private Material material;

    void Start ()
    {
        // spriteRenderer.color = MColor.orange; // Set to opaque black
        // spriteRenderer.material.SetColor("_Color", MColor.orange);
        SetAnimations();
    }

    void SetAnimations()
    {
        idleAnimation = name + "_Idle";
        runAnimation = name + "_Run";
        jumpAnimation = name + "_Jump";
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
}