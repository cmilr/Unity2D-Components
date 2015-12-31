using DG.Tweening;
using Matcha.Dreadful;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

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
		Evnt.Subscribe<int>("init score", OnInitInteger);
		Evnt.Subscribe<int>("change score", OnChangeInteger);
		Evnt.Subscribe<bool>("fade hud", OnFadeHud);
	}

	void OnDestroy()
	{
		Evnt.Unsubscribe<int>("init score", OnInitInteger);
		Evnt.Unsubscribe<int>("change score", OnChangeInteger);
		Evnt.Unsubscribe<bool>("fade hud", OnFadeHud);
	}
}
