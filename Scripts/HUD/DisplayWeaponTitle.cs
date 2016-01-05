using DG.Tweening;
using Matcha.Dreadful;
using UnityEngine.UI;
using UnityEngine;

public class DisplayWeaponTitle : BaseBehaviour
{
	private Text textComponent;

	void OnInitEquippedWeapon(GameObject weapon)
	{
		textComponent      = gameObject.GetComponent<Text>();
		textComponent.text = weapon.GetComponent<Weapon>().title;
		textComponent.DOKill();
		FadeInInitialTitle();
	}

	void OnInitNewEquippedWeapon(GameObject weapon)
	{
		textComponent      = gameObject.GetComponent<Text>();
		textComponent.text = weapon.GetComponent<Weapon>().title;
		textComponent.DOKill();
		// FadeInInitialTitle();
	}

	void FadeInInitialTitle()
	{
		// fade to zero instantly, then fade up slowly
		MFX.Fade(textComponent, 0, 0, 0);
		MFX.Fade(textComponent, 1, HUD_FADE_IN_AFTER, HUD_INITIAL_TIME_TO_FADE);
	}

	void OnChangeEquippedWeapon(GameObject newWeapon)
	{
		textComponent.text = newWeapon.GetComponent<Weapon>().title;
		textComponent.DOKill();
		FadeInNewTitle();
	}

	void FadeInNewTitle()
	{
		// fade to zero instantly, then fade up slowly
		MFX.Fade(textComponent, 0, 0, 0);
		MFX.Fade(textComponent, 1, 0, HUD_WEAPON_CHANGE_FADE);
	}

	void OnFadeHud(bool status)
	{
		MFX.Fade(textComponent, 0, HUD_FADE_OUT_AFTER, HUD_INITIAL_TIME_TO_FADE);
	}

	void OnEnable()
	{
		EventKit.Subscribe<GameObject>("init equipped weapon", OnInitEquippedWeapon);
		EventKit.Subscribe<GameObject>("init new equipped weapon", OnInitNewEquippedWeapon);
		EventKit.Subscribe<GameObject>("change equipped weapon", OnChangeEquippedWeapon);
		EventKit.Subscribe<bool>("fade hud", OnFadeHud);
	}

	void OnDestroy()
	{
		EventKit.Unsubscribe<GameObject>("init equipped weapon", OnInitEquippedWeapon);
		EventKit.Unsubscribe<GameObject>("init new equipped weapon", OnInitNewEquippedWeapon);
		EventKit.Unsubscribe<GameObject>("change equipped weapon", OnChangeEquippedWeapon);
		EventKit.Unsubscribe<bool>("fade hud", OnFadeHud);
	}
}
