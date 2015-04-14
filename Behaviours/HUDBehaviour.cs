using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using Matcha.Game.Tweens;


public class HUDBehaviour : CacheBehaviour
{
    public Sprite spriteElement;
    private Camera mainCamera;
    private SpriteRenderer HUDElement;
    private float timeToFade = 2f;

    void Start()
    {
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        PositionHUDElement();

        HUDElement = spriteRenderer;
        HUDElement.sprite = spriteElement;
        HUDElement.DOKill();
        FadeInElement();
    }

    void FadeInElement()
    {
        // fade weapon to zero instantly, then fade up slowly
        MTween.FadeOut(HUDElement, 0, 0);
        MTween.FadeIn(HUDElement, HUD_FADE_IN_AFTER, timeToFade);
    }

    void OnFadeHud(bool status)
    {
        MTween.FadeOut(HUDElement, HUD_FADE_OUT_AFTER, timeToFade);
    }

    void OnScreenSizeChanged(float vExtent, float hExtent)
    {
        PositionHUDElement();
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
