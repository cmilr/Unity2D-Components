using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using Matcha.Dreadful;


// this is a template for basic integer stats that need to be displayed
// instead of using this actual file, copy & paste this code into a new component
public class DisplayInteger : BaseBehaviour
{
    private Text textComponent;
    private int intToDisplay;
    private string legend = "XP: ";

    void Start()
    {
        textComponent = gameObject.GetComponent<Text>();
        textComponent.DOKill();
    }

    void FadeInText()
    {
        // fade to zero instantly, then fade up slowly
        MFX.Fade(textComponent, 0, 0, 0);
        MFX.Fade(textComponent, 1, HUD_FADE_IN_AFTER, HUD_INITIAL_TIME_TO_FADE);
    }

    void OnInitInteger(int initInt)
    {
        textComponent.text = legend + initInt.ToString();
        FadeInText();
    }

    void OnChangeInteger(int newInt)
    {
        textComponent.text = legend + newInt.ToString();
    }

    void OnFadeHud(bool status)
    {
        MFX.Fade(textComponent, 0, HUD_FADE_OUT_AFTER, HUD_INITIAL_TIME_TO_FADE);
    }

    void OnEnable()
    {
        Messenger.AddListener<int>("init score", OnInitInteger);
        Messenger.AddListener<int>("change score", OnChangeInteger);
        Messenger.AddListener<bool>("fade hud", OnFadeHud);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener<int>("init score", OnInitInteger);
        Messenger.RemoveListener<int>("change score", OnChangeInteger);
        Messenger.RemoveListener<bool>("fade hud", OnFadeHud);
    }
}