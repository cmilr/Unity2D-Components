using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using Matcha.Game.Tweens;


public class DisplayWeaponTitle : BaseBehaviour
{
    private Text textComponent;

    void Start()
    {
        textComponent = gameObject.GetComponent<Text>();
        textComponent.DOKill();
    }

    void FadeInText()
    {
        // fade to zero instantly, then fade up slowly
        MTween.FadeOut(textComponent, 0, 0);
        MTween.FadeIn(textComponent, HUD_FADE_IN_AFTER, HUD_TIME_TO_FADE);
    }

    void OnInitWeaponTitle(GameObject weapon)
    {
        textComponent.text = weapon.GetComponent<Sword>().title;
        FadeInText();
    }

    void OnChangeWeaponTitle(GameObject newWeapon)
    {
        textComponent.text = newWeapon.GetComponent<Sword>().title;

        // MTween.DisplayScore(gameObject, textComponent);
    }

    void OnFadeHud(bool status)
    {
        MTween.FadeOut(textComponent, HUD_FADE_OUT_AFTER, HUD_TIME_TO_FADE);
    }

    void OnEnable()
    {
        Messenger.AddListener<GameObject>("init weapon title", OnInitWeaponTitle);
        // Messenger.AddListener<GameObject>("change weapon title", OnChangeWeaponTitle);
        Messenger.AddListener<bool>("fade hud", OnFadeHud);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener<GameObject>("init weapon title", OnInitWeaponTitle);
        // Messenger.RemoveListener<GameObject>("change weapon title", OnChangeWeaponTitle);
        Messenger.RemoveListener<bool>("fade hud", OnFadeHud);
    }
}