using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using Matcha.Game.Tweens;


public class DisplayAC : BaseBehaviour
{
    private Text textComponent;
    private int intToDisplay;
    private string legend = "AC: ";

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

    void OnInitInteger(int initInt)
    {
        textComponent.text = legend + initInt.ToString();
        FadeInText();
    }

    void OnChangeInteger(int newInt)
    {
        textComponent.text = legend + newInt.ToString();

        // MTween.DisplayScore(gameObject, textComponent);
    }

    void OnFadeHud(bool status)
    {
        MTween.FadeOut(textComponent, HUD_FADE_OUT_AFTER, HUD_TIME_TO_FADE);
    }

    void OnEnable()
    {
        Messenger.AddListener<int>("init ac", OnInitInteger);
        // Messenger.AddListener<int>("change score", OnChangeInteger);
        Messenger.AddListener<bool>("fade hud", OnFadeHud);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener<int>("init ac", OnInitInteger);
        // Messenger.RemoveListener<int>("change score", OnChangeInteger);
        Messenger.RemoveListener<bool>("fade hud", OnFadeHud);
    }
}