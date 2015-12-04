using DG.Tweening;
using Matcha.Dreadful;
using System.Collections;
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
		Messenger.AddListener<GameObject>("init equipped weapon", OnInitEquippedWeapon);
		Messenger.AddListener<GameObject>("init new equipped weapon", OnInitNewEquippedWeapon);
		Messenger.AddListener<GameObject>("change equipped weapon", OnChangeEquippedWeapon);
		Messenger.AddListener<bool>("fade hud", OnFadeHud);
	}

	void OnDestroy()
	{
		Messenger.RemoveListener<GameObject>("init equipped weapon", OnInitEquippedWeapon);
		Messenger.RemoveListener<GameObject>("init new equipped weapon", OnInitNewEquippedWeapon);
		Messenger.RemoveListener<GameObject>("change equipped weapon", OnChangeEquippedWeapon);
		Messenger.RemoveListener<bool>("fade hud", OnFadeHud);
	}
}
