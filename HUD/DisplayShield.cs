using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using Matcha.Game.Tweens;


public class DisplayShield : BaseBehaviour
{
    public Sprite equippedShield;
    private Image HUDShield;
    private float timeToFade = 2f;

    void Start()
    {
        HUDShield = gameObject.GetComponent<Image>();
        HUDShield.sprite = equippedShield;
        HUDShield.DOKill();
        FadeInShield();
    }

    void FadeInShield()
    {
        // fade shield to zero instantly, then fade up slowly
        MTween.FadeOut(HUDShield, 0, 0);
        MTween.FadeIn(HUDShield, HUD_FADE_IN_AFTER, timeToFade);
    }

    void OnFadeHud(bool status)
    {
        MTween.FadeOut(HUDShield, HUD_FADE_OUT_AFTER, timeToFade);
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
