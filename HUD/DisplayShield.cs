using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using Matcha.Game.Tweens;


public class DisplayShield : CacheBehaviour
{
    public Sprite equippedShield;
    private Camera mainCamera;
    private SpriteRenderer HUDShield;
    private float timeToFade = 2f;

    void Start()
    {
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        PositionHUDElements();

        HUDShield = spriteRenderer;
        HUDShield.sprite = equippedShield;
        HUDShield.DOKill();
        FadeInShield();
    }

    void PositionHUDElements()
    {
        transform.position = mainCamera.ScreenToWorldPoint(new Vector3 (Screen.width / 2, Screen.height - HUD_EQUIPPED_TOP_MARGIN, HUD_Z));
    }

    void FadeInShield()
    {
        // fade weapon to zero instantly, then fade up slowly
        MTween.FadeOut(HUDShield, 0, 0);
        MTween.FadeIn(HUDShield, HUD_FADE_IN_AFTER, timeToFade);
    }

    void OnFadeHud(bool status)
    {
        MTween.FadeOut(HUDShield, HUD_FADE_OUT_AFTER, timeToFade);
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
