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

    void Start()
    {
        mainCamera = Camera.main.GetComponent<Camera>();

        HUDHearts = spriteRenderer;
        HUDHearts.sprite = threeHearts;
        HUDHearts.DOKill();
        FadeInShield();

        Invoke("PositionHUDElements", .001f);
    }

    void PositionHUDElements()
    {
        transform.position = mainCamera.ScreenToWorldPoint(new Vector3(
            Screen.width - 112,
            Screen.height - 70,
            HUD_Z));
    }

    void FadeInShield()
    {
        // fade to zero instantly, then fade up slowly
        MTween.FadeOut(HUDHearts, 0, 0);
        MTween.FadeIn(HUDHearts, HUD_FADE_IN_AFTER, HUD_INITIAL_TIME_TO_FADE);
    }

    void OnFadeHud(bool status)
    {
        MTween.FadeOut(HUDHearts, HUD_FADE_OUT_AFTER, HUD_INITIAL_TIME_TO_FADE);
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
