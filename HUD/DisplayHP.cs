using DG.Tweening;
using Matcha.Dreadful;
using UnityEngine.UI;

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
		EventKit.Subscribe<int>("init hp", OnInitHP);
		EventKit.Subscribe<int>("reduce hp", OnReduceHP);
		EventKit.Subscribe<bool>("fade hud", OnFadeHud);
	}

	void OnDestroy()
	{
		EventKit.Unsubscribe<int>("init hp", OnInitHP);
		EventKit.Unsubscribe<int>("reduce hp", OnReduceHP);
		EventKit.Unsubscribe<bool>("fade hud", OnFadeHud);
	}
}
