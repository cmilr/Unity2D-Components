using DG.Tweening;
using Matcha.Dreadful;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class HudTextBehaviour : BaseBehaviour
{
	private Text text;
	private CanvasScaler canvasScaler;
	private RectTransform rectTrans;
	private Camera cam;
	private Sequence fadeIn;
	private Sequence fadeOutInstant;
	private Sequence fadeOutHUD;
	private float canvasScale;
	protected Position anchorPosition;
	protected float targetXPos;
	protected float targetYPos;
	protected float resolutionMultiplier;
	protected int intToDisplay = 1;
	protected string legend;

	protected void BaseAwake()
	{
		rectTrans = GetComponent<RectTransform>();
		Assert.IsNotNull(rectTrans);

		text = GetComponent<Text>();
		Assert.IsNotNull(text);
	}

	protected void BaseStart()
	{
		canvasScaler = gameObject.GetComponentInParent<CanvasScaler>();
		Assert.IsNotNull(canvasScaler);

		canvasScale = Camera.main.GetComponent<PixelArtCamera>().CanvasScale;
		Assert.AreNotApproximatelyEqual(0.0f, canvasScale);

		resolutionMultiplier = Camera.main.GetComponent<PixelArtCamera>().FinalUnitSize;
		Assert.AreNotApproximatelyEqual(0.0f, resolutionMultiplier);

		cam = Camera.main.GetComponent<Camera>();
		Assert.IsNotNull(cam);

		PositionHUDElements();
		text.enabled = true;

		// cache & pause tween sequences.
		(fadeOutInstant = MFX.Fade(text, 0, 0, 0)).Pause();
		(fadeIn 		= MFX.Fade(text, 1, HUD_FADE_IN_AFTER, HUD_INITIAL_FADE_LENGTH)).Pause();
		(fadeOutHUD 	= MFX.Fade(text, 0, HUD_FADE_OUT_AFTER, HUD_INITIAL_FADE_LENGTH)).Pause();
	}

	protected void BaseOnInitInteger(int initInt)
	{
		text.text = legend + initInt;
		FadeInText();
	}

	void PositionHUDElements()
	{
		canvasScaler.scaleFactor = canvasScale;
		PositionInHUD();
	}

	protected void PositionInHUD()
	{
		// with a text and sprite hud element placed in the same position,
		// these nudge variables allow text to be as closely aligned as
		// possible to the sprite, pixel for pixel. not perfect, but close.
		var nudgeX = .65225f;
		var nudgeY = 1f;

		// this is a truly magic number. i hope to track down its origin,
		// but it seems to work at all screen sizes tested, so it's a go.
		var mysteryFactor = 128f;

		// NOTE: positioning takes place AFTER this switch statement.
		switch (anchorPosition)
		{
			case Position.TopLeft:
				text.alignment 		= TextAnchor.UpperLeft;
				rectTrans.pivot 	= new Vector2(0f, 1f);
				rectTrans.anchorMin = new Vector2(0f, 1f);
				rectTrans.anchorMax = new Vector2(0f, 1f);
				nudgeX = -nudgeX;
				break;
			case Position.TopCenter:
				text.alignment 		= TextAnchor.UpperCenter;
				rectTrans.pivot 	= new Vector2(0.5f, 1f);
				rectTrans.anchorMin = new Vector2(0.5f, 1f);
				rectTrans.anchorMax = new Vector2(0.5f, 1f);
				break;
			case Position.TopRight:
				text.alignment 		= TextAnchor.UpperRight;
				rectTrans.pivot 	= new Vector2(1f, 1f);
				rectTrans.anchorMin = new Vector2(1f, 1f);
				rectTrans.anchorMax = new Vector2(1f, 1f);
				break;
			case Position.MiddleLeft:
				text.alignment 		= TextAnchor.MiddleLeft;
				rectTrans.pivot 	= new Vector2(0f, 0.5f);
				rectTrans.anchorMin = new Vector2(0f, 0.5f);
				rectTrans.anchorMax = new Vector2(0f, 0.5f);
				nudgeX = -nudgeX;
				nudgeY = -nudgeY;
				break;
			case Position.MiddleCenter:
				text.alignment 		= TextAnchor.MiddleCenter;
				rectTrans.pivot 	= new Vector2(0.5f, 0.5f);
				rectTrans.anchorMin = new Vector2(0.5f, 0.5f);
				rectTrans.anchorMax = new Vector2(0.5f, 0.5f);
				nudgeY = -nudgeY;
				break;
			case Position.MiddleRight:
				text.alignment 		= TextAnchor.MiddleRight;
				rectTrans.pivot 	= new Vector2(1f, 0.5f);
				rectTrans.anchorMin = new Vector2(1f, 0.5f);
				rectTrans.anchorMax = new Vector2(1f, 0.5f);
				nudgeY = -nudgeY;
				break;
			case Position.BottomLeft:
				text.alignment 		= TextAnchor.LowerLeft;
				rectTrans.pivot 	= new Vector2(0f, 0f);
				rectTrans.anchorMin = new Vector2(0f, 0f);
				rectTrans.anchorMax = new Vector2(0f, 0f);
				nudgeX = -nudgeX;
				nudgeY = -nudgeY;
				break;
			case Position.BottomCenter:
				text.alignment 		= TextAnchor.LowerCenter;
				rectTrans.pivot 	= new Vector2(0.5f, 0f);
				rectTrans.anchorMin = new Vector2(0.5f, 0f);
				rectTrans.anchorMax = new Vector2(0.5f, 0f);
				nudgeY = -nudgeY;
				break;
			case Position.BottomRight:
				text.alignment 		= TextAnchor.LowerRight;
				rectTrans.pivot 	= new Vector2(1f, 0f);
				rectTrans.anchorMin = new Vector2(1f, 0f);
				rectTrans.anchorMax = new Vector2(1f, 0f);
				nudgeY = -nudgeY;
				break;
			default:
				Assert.IsTrue(false, "** Default Case Reached **");
				break;
		}

		// position text element
		rectTrans.anchoredPosition = new Vector3(
			targetXPos * mysteryFactor + nudgeX,
			targetYPos * mysteryFactor + nudgeY, HUD_Z);
	}

	void FadeInText()
	{
		fadeOutInstant.Restart();
		fadeIn.Restart();
	}

	void OnChangeInteger(int newInt)
	{
		text.text = legend + newInt;
	}

	void OnFadeHud(bool status)
	{
		fadeOutHUD.Restart();
	}

	void OnEnable()
	{
		EventKit.Subscribe<bool>("fade hud", OnFadeHud);
		EventKit.Subscribe("reposition hud elements", PositionHUDElements);
	}

	void OnDestroy()
	{
		EventKit.Unsubscribe<bool>("fade hud", OnFadeHud);
		EventKit.Unsubscribe("reposition hud elements", PositionHUDElements);
	}
}
