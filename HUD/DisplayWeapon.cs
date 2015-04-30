using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using Matcha.Game.Tweens;


public class DisplayWeapon : CacheBehaviour
{
    public enum WeaponPosition { Equipped, Left, Right };
    public WeaponPosition weaponPosition;

    public Sprite equippedWeapon;
    public float offset;
    private Camera mainCamera;
    private SpriteRenderer HUDWeapon;

    void Start()
    {
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();

        HUDWeapon = spriteRenderer;
        HUDWeapon.sprite = equippedWeapon;
        HUDWeapon.DOKill();
        // FadeInWeapon();


        Invoke("PositionHUDElements", .1f);
        HUDWeapon.color = new Color (.5f, .5f, .5f, .3f);
    }

    void PositionHUDElements()
    {
        transform.position = mainCamera.ScreenToWorldPoint(new Vector3(
            Screen.width / 2 + offset,
            Screen.height - HUD_EQUIPPED_TOP_MARGIN,
            HUD_Z));
    }

    void FadeInWeapon()
    {
        // fade weapon to zero instantly, then fade up slowly
        MTween.FadeOut(HUDWeapon, 0, 0);
        MTween.FadeIn(HUDWeapon, HUD_FADE_IN_AFTER, HUD_TIME_TO_FADE);
    }

    void OnFadeHud(bool status)
    {
        MTween.FadeOut(HUDWeapon, HUD_FADE_OUT_AFTER, HUD_TIME_TO_FADE);
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
