using DG.Tweening;
using Matcha.Dreadful;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class DisplayShield : CacheBehaviour
{
	public Sprite equippedShield;
	private Camera mainCamera;
	private SpriteRenderer HUDShield;

	void Start()
	{
		mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
		HUDShield = spriteRenderer;
		HUDShield.sprite = equippedShield;
		HUDShield.DOKill();
		FadeInShield();
		PositionHUDElements();
	}

	void PositionHUDElements()
	{
		transform.position = mainCamera.ScreenToWorldPoint(new Vector3(
							Screen.width / 2,
							Screen.height - HUD_WEAPON_TOP_MARGIN,
							HUD_Z)
						);
	}

	void FadeInShield()
	{
		// fade weapon to zero instantly, then fade up slowly
		MFX.Fade(HUDShield, 0, 0, 0);
		MFX.Fade(HUDShield, 1, HUD_FADE_IN_AFTER, HUD_INITIAL_TIME_TO_FADE);
	}

	void OnFadeHud(bool status)
	{
		MFX.Fade(HUDShield, 0, HUD_FADE_OUT_AFTER, HUD_INITIAL_TIME_TO_FADE);
	}

	void OnScreenSizeChanged(float vExtent, float hExtent)
	{
		PositionHUDElements();
	}

	void OnEnable()
	{
		Evnt.Subscribe<bool>("fade hud", OnFadeHud);
		Evnt.Subscribe<float, float>("screen size changed", OnScreenSizeChanged);
	}

	void OnDestroy()
	{
		Evnt.Unsubscribe<bool>("fade hud", OnFadeHud);
		Evnt.Unsubscribe<float, float>("screen size changed", OnScreenSizeChanged);
	}
}
