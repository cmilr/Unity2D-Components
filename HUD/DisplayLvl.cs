using DG.Tweening;
using Matcha.Dreadful;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class DisplayLvl : BaseBehaviour
{
	private Text textComponent;
	private int intToDisplay;
	private string legend = "Lvl: ";

	void FadeInText()
	{
		// fade to zero instantly, then fade up slowly
		MFX.Fade(textComponent, 0, 0, 0);
		MFX.Fade(textComponent, 1, HUD_FADE_IN_AFTER, HUD_INITIAL_TIME_TO_FADE);
	}

	void OnInitInteger(int initInt)
	{
		textComponent = gameObject.GetComponent<Text>();
		textComponent.text = legend + initInt.ToString();
		textComponent.DOKill();
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
		Evnt.Subscribe<int>("init lvl", OnInitInteger);
		// Evnt.Subscribe<int>("change score", OnChangeInteger);
		Evnt.Subscribe<bool>("fade hud", OnFadeHud);
	}

	void OnDestroy()
	{
		Evnt.Unsubscribe<int>("init lvl", OnInitInteger);
		// Evnt.Unsubscribe<int>("change score", OnChangeInteger);
		Evnt.Unsubscribe<bool>("fade hud", OnFadeHud);
	}
}
