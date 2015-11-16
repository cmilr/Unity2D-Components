using DG.Tweening;
using Matcha.Dreadful;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class DisplayHP : BaseBehaviour
{
	private Text textComponent;
	private string legend = "HP: ";

	void FadeInText()
	{
		// fade to zero instantly, then fade up slowly
		MFX.Fade(textComponent, 0, 0, 0);
		MFX.Fade(textComponent, 1, HUD_FADE_IN_AFTER, HUD_INITIAL_TIME_TO_FADE);
	}

	void OnInitHP(int initInt)
	{
		textComponent = gameObject.GetComponent<Text>();
		textComponent.text = legend + initInt.ToString();
		textComponent.DOKill();
		FadeInText();
	}

	void OnReduceHP(int newInt)
	{
		textComponent.text = legend + newInt.ToString();
	}

	void OnFadeHud(bool status)
	{
		MFX.Fade(textComponent, 0, HUD_FADE_OUT_AFTER, HUD_INITIAL_TIME_TO_FADE);
	}

	void OnEnable()
	{
		Messenger.AddListener<int>("init hp", OnInitHP);
		Messenger.AddListener<int>("reduce hp", OnReduceHP);
		Messenger.AddListener<bool>("fade hud", OnFadeHud);
	}

	void OnDestroy()
	{
		Messenger.RemoveListener<int>("init hp", OnInitHP);
		Messenger.RemoveListener<int>("reduce hp", OnReduceHP);
		Messenger.RemoveListener<bool>("fade hud", OnFadeHud);
	}
}
