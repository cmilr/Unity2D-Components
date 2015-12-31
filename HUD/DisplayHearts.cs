using DG.Tweening;
using Matcha.Dreadful;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class DisplayHearts : CacheBehaviour
{
	public Sprite threeHearts;
	public Sprite twoHearts;
	public Sprite oneHearts;
	private Camera mainCamera;
	private SpriteRenderer HUDHearts;

	void Start()
	{
		mainCamera = Camera.main.GetComponent<Camera>();

		HUDHearts = spriteRenderer;
		HUDHearts.sprite = threeHearts;
		HUDHearts.DOKill();
		FadeInShield();

		Invoke("PositionHUDElements", .001f);
	}

	void PositionHUDElements()
	{
		transform.position = mainCamera.ScreenToWorldPoint(new Vector3(
							Screen.width - 112,
							Screen.height - 70,
							HUD_Z)
						);
	}

	void FadeInShield()
	{
		// fade to zero instantly, then fade up slowly
		MFX.Fade(HUDHearts, 0, 0, 0);
		MFX.Fade(HUDHearts, 1, HUD_FADE_IN_AFTER, HUD_INITIAL_TIME_TO_FADE);
	}

	void OnFadeHud(bool status)
	{
		MFX.Fade(HUDHearts, 0, HUD_FADE_OUT_AFTER, HUD_INITIAL_TIME_TO_FADE);
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
