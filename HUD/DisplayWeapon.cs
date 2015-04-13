using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using Matcha.Game.Tweens;


public class DisplayWeapon : BaseBehaviour
{
    public Sprite equippedWeapon;
    private Image HUDWeapon;
    private float timeToFade = 2f;

    void Start()
    {
        HUDWeapon = gameObject.GetComponent<Image>();
        HUDWeapon.sprite = equippedWeapon;
        HUDWeapon.DOKill();
        FadeInWeapon();
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

    void OnEnable()
    {
        Messenger.AddListener<bool>("fade hud", OnFadeHud);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener<bool>("fade hud", OnFadeHud);
    }
}
