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
		Evnt.Subscribe<GameObject>("init equipped weapon", OnInitEquippedWeapon);
		Evnt.Subscribe<GameObject>("init new equipped weapon", OnInitNewEquippedWeapon);
		Evnt.Subscribe<GameObject>("change equipped weapon", OnChangeEquippedWeapon);
		Evnt.Subscribe<bool>("fade hud", OnFadeHud);
	}

	void OnDestroy()
	{
		Evnt.Unsubscribe<GameObject>("init equipped weapon", OnInitEquippedWeapon);
		Evnt.Unsubscribe<GameObject>("init new equipped weapon", OnInitNewEquippedWeapon);
		Evnt.Unsubscribe<GameObject>("change equipped weapon", OnChangeEquippedWeapon);
		Evnt.Unsubscribe<bool>("fade hud", OnFadeHud);
	}
}
