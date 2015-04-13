using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using Matcha.Game.Tweens;


public class DisplayHearts : CacheBehaviour
{

	public Sprite threeLives;
	public Sprite twoLives;
	public Sprite oneLife;
    private Camera mainCamera;
    private SpriteRenderer HUDHearts;
    private float timeToFade = 2f;

    void Awake ()
    {
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        transform.position = mainCamera.ScreenToWorldPoint(new Vector3 (Screen.width - 20, Screen.height - HUD_HEARTS_TOP_MARGIN, HUD_Z));
    }

    void Start()
    {
        HUDHearts = spriteRenderer;
        HUDHearts.sprite = threeLives;
        HUDHearts.DOKill();
        FadeInHearts();
    }

    void FadeInHearts()
    {
        // fade weapon to zero instantly, then fade up slowly
        MTween.FadeOut(HUDHearts, 0, 0);
        MTween.FadeIn(HUDHearts, HUD_FADE_IN_AFTER, timeToFade);
    }

    void OnFadeHud(bool status)
    {
        MTween.FadeOut(HUDHearts, HUD_FADE_OUT_AFTER, timeToFade);
    }

    void OnEnable()
    {
        Messenger.AddListener<bool>("fade hud", OnFadeHud);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener<bool>("fade hud", OnFadeHud);
    }
}