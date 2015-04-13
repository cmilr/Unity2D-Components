using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using Matcha.Game.Tweens;


public class DisplayWeapon : CacheBehaviour
{
    public Sprite equippedWeapon;
    private Camera mainCamera;
    private SpriteRenderer HUDWeapon;
    private float timeToFade = 2f;

    void Start()
    {
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        PositionHUDElements();

        HUDWeapon = spriteRenderer;
        HUDWeapon.sprite = equippedWeapon;
        HUDWeapon.DOKill();
        FadeInWeapon();
    }

    void PositionHUDElements()
    {
        transform.position = mainCamera.ScreenToWorldPoint(new Vector3 (Screen.width / 2, Screen.height - HUD_TOP_MARGIN, HUD_Z));
    }

    void FadeInWeapon()
    {
        // fade weapon to zero instantly, then fade up slowly
        MTween.FadeOut(HUDWeapon, 0, 0);
        MTween.FadeIn(HUDWeapon, HUD_FADE_IN_AFTER, timeToFade);
    }

    void OnFadeHud(bool status)
    {
        MTween.FadeOut(HUDWeapon, HUD_FADE_OUT_AFTER, timeToFade);
    }

    void OnScreenSizeChanged(float vExtent, float hExtent)
    {
        PositionHUDElements();
    }

    void OnEnable()
    {
        Messenger.AddListener<bool>("fade hud", OnFadeHud);
        Messenger.AddListener<float, float>( "screen size changed", OnScreenSizeChanged);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener<bool>("fade hud", OnFadeHud);
        Messenger.RemoveListener<float, float>( "screen size changed", OnScreenSizeChanged);
    }
}
