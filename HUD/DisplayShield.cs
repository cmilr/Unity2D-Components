using DG.Tweening;
using Matcha.Dreadful;
using UnityEngine;
using UnityEngine.Assertions;

public class DisplayShield : BaseBehaviour
{
	public Sprite equippedShield;
	private Camera mainCamera;
	private SpriteRenderer spriteRenderer;
	private Sequence fadeOutHUD;
	private Sequence fadeOutInstant;
	private Sequence fadeIn;

	void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		Assert.IsNotNull(spriteRenderer);
	}
	
	void Start()
	{
		mainCamera = Camera.main.GetComponent<Camera>();
		Assert.IsNotNull(mainCamera);
		
		// cache & pause tween sequences.
		(fadeOutHUD         = MFX.Fade(spriteRenderer, 0, HUD_FADE_OUT_AFTER, HUD_INITIAL_TIME_TO_FADE)).Pause();
		(fadeOutInstant     = MFX.Fade(spriteRenderer, 0, 0, 0)).Pause();
		(fadeIn             = MFX.Fade(spriteRenderer, 1, HUD_FADE_IN_AFTER, HUD_INITIAL_TIME_TO_FADE)).Pause();
		
		spriteRenderer.sprite = equippedShield;

		PositionHUDElements();
		FadeInShield();
	}

	void PositionHUDElements()
	{
		transform.position = mainCamera.ScreenToWorldPoint(new Vector3(
			Screen.width / 2,
			Screen.height - INVENTORY_Y_POS,
			HUD_Z)
		);
	}

	void FadeInShield()
	{
		fadeOutInstant.Restart();
		fadeIn.Restart();
	}

	void OnFadeHud(bool status)
	{
		fadeOutHUD.Restart();
	}

	void OnScreenSizeChanged(float vExtent, float hExtent)
	{
		PositionHUDElements();
	}

	void OnEnable()
	{
		EventKit.Subscribe<bool>("fade hud", OnFadeHud);
		EventKit.Subscribe<float, float>("screen size changed", OnScreenSizeChanged);
	}

	void OnDestroy()
	{
		EventKit.Unsubscribe<bool>("fade hud", OnFadeHud);
		EventKit.Unsubscribe<float, float>("screen size changed", OnScreenSizeChanged);
	}
}
