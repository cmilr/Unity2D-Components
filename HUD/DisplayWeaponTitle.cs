using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using Matcha.Game.Tweens;


public class DisplayWeaponTitle : BaseBehaviour
{
    private Text textComponent;
    private int intToDisplay;
    private string legend = "";

    void Awake()
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

    void OnInitWeaponTitle(string weaponTitle)
    {
        textComponent.text = legend + weaponTitle;
        FadeInText();
    }

    void OnChangeWeaponTitle(int newWeaponTitle)
    {
        textComponent.text = legend + newWeaponTitle.ToString();

        // MTween.DisplayScore(gameObject, textComponent);
    }

    void OnFadeHud(bool status)
    {
        MTween.FadeOut(textComponent, HUD_FADE_OUT_AFTER, HUD_TIME_TO_FADE);
    }

    void OnEnable()
    {
        Messenger.AddListener<string>("init weapon title", OnInitWeaponTitle);
        // Messenger.AddListener<string>("change weapon title", OnChangeWeaponTitle);
        Messenger.AddListener<bool>("fade hud", OnFadeHud);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener<string>("init weapon title", OnInitWeaponTitle);
        // Messenger.RemoveListener<string>("change weapon title", OnChangeWeaponTitle);
        Messenger.RemoveListener<bool>("fade hud", OnFadeHud);
    }
}