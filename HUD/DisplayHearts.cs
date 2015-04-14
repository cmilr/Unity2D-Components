using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using Matcha.Game.Tweens;


public class DisplayHearts : CacheBehaviour
{
    public Sprite threeHearts;
    public Sprite twoHearts;
    public Sprite oneHearts;
    private Camera mainCamera;
    private SpriteRenderer HUDHearts;
    private float timeToFade = 2f;

    void Start()
    {
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        PositionHUDElements();

        HUDHearts = spriteRenderer;
        HUDHearts.sprite = threeHearts;
        HUDHearts.DOKill();
        FadeInShield();
    }

    void PositionHUDElements()
    {
        transform.position = mainCamera.ScreenToWorldPoint(new Vector3 (Screen.width - 60, Screen.height - HUD_TOP_MARGIN + 60, HUD_Z));
    }

    void FadeInShield()
    {
        // fade weapon to zero instantly, then fade up slowly
        MTween.FadeOut(HUDHearts, 0, 0);
        MTween.FadeIn(HUDHearts, HUD_FADE_IN_AFTER, timeToFade);
    }

    void OnFadeHud(bool status)
    {
        MTween.FadeOut(HUDHearts, HUD_FADE_OUT_AFTER, timeToFade);
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
