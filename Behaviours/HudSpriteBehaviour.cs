using UnityEngine;
using UnityEngine.Assertions;
using DG.Tweening;
using Matcha.Dreadful;

public class HudSpriteBehaviour : BaseBehaviour 
{
	protected new Transform transform;
	protected Camera cam;
	protected SpriteRenderer spriteRenderer;
	protected Sequence fadeInHUD;
	protected Sequence fadeOutHUD;
	protected Sequence fadeOutInstant;
	protected Position anchorPosition;
	protected float targetXPos;
	protected float targetYPos;
	protected float resolutionMultiplier;

	protected void BaseAwake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		Assert.IsNotNull(spriteRenderer);

		transform = GetComponent<Transform>();
		Assert.IsNotNull(transform);
	}

	protected void BaseStart()
	{
		cam = Camera.main.GetComponent<Camera>();
		Assert.IsNotNull(cam);

		resolutionMultiplier = Camera.main.GetComponent<PixelArtCamera>().FinalUnitSize;
		Assert.AreNotApproximatelyEqual(0.0f, resolutionMultiplier);

		// cache & pause tween sequences.
		(fadeInHUD 		= MFX.Fade(spriteRenderer, 1, HUD_FADE_IN_AFTER,  HUD_INITIAL_FADE_LENGTH)).Pause();
		(fadeOutHUD 	= MFX.Fade(spriteRenderer, 0, HUD_FADE_OUT_AFTER, HUD_INITIAL_FADE_LENGTH)).Pause();
		(fadeOutInstant = MFX.Fade(spriteRenderer, 0, 0, 0)).Pause();

		PositionHUDElements();
		spriteRenderer.enabled = true;
		FadeIn();
	}

	void PositionHUDElements()
	{
		PositionInHUD();
	}

	void FadeIn()
	{
		fadeOutInstant.Restart();
		fadeInHUD.Restart();
	}

	void OnFadeHud(bool status)
	{
		fadeOutHUD.Restart();
	}

	protected void PositionInHUD()
	{
		float xPos;
		float yPos;

		switch (anchorPosition)
		{
			case Position.TopLeft:
				xPos = (targetXPos + (spriteRenderer.bounds.size.x / 2));
				yPos = (targetYPos + (spriteRenderer.bounds.size.y));
				transform.position = cam.ScreenToWorldPoint(new Vector3(
					xPos * resolutionMultiplier,
					Screen.height - (yPos * resolutionMultiplier) + (targetYPos * 2 * resolutionMultiplier),
					HUD_Z)
				);
				break;
			case Position.TopCenter:
				xPos = targetXPos;
				yPos = (targetYPos + (spriteRenderer.bounds.size.y));
				transform.position = cam.ScreenToWorldPoint(new Vector3(
					Screen.width / 2 + (xPos * resolutionMultiplier),
					Screen.height - (yPos * resolutionMultiplier) + (targetYPos * 2 * resolutionMultiplier),
					HUD_Z)
				);
				break;
			case Position.TopRight:
				xPos = (targetXPos + (spriteRenderer.bounds.size.x / 2));
				yPos = (targetYPos + (spriteRenderer.bounds.size.y));
				transform.position = cam.ScreenToWorldPoint(new Vector3(
					Screen.width - (xPos * resolutionMultiplier) + (targetXPos * 2 * resolutionMultiplier),
					Screen.height - (yPos * resolutionMultiplier) + (targetYPos * 2 * resolutionMultiplier),
					HUD_Z)
				);
				break;
			case Position.MiddleLeft:
				xPos = (targetXPos + (spriteRenderer.bounds.size.x / 2));
				yPos = (targetYPos + (spriteRenderer.bounds.size.y / 2));
				transform.position = cam.ScreenToWorldPoint(new Vector3(
					xPos * resolutionMultiplier,
					Screen.height / 2 - (yPos * resolutionMultiplier) + (targetYPos * 2 * resolutionMultiplier),
					HUD_Z)
				);
				break;
			case Position.MiddleCenter:
				xPos = targetXPos;
				yPos = (targetYPos + (spriteRenderer.bounds.size.y / 2));
				transform.position = cam.ScreenToWorldPoint(new Vector3(
					Screen.width / 2 + (xPos * resolutionMultiplier),
					Screen.height / 2 - (yPos * resolutionMultiplier) + (targetYPos * 2 * resolutionMultiplier),
					HUD_Z)
				);
				break;
			case Position.MiddleRight:
				xPos = (targetXPos + (spriteRenderer.bounds.size.x / 2));
				yPos = (targetYPos + (spriteRenderer.bounds.size.y / 2));
				transform.position = cam.ScreenToWorldPoint(new Vector3(
					Screen.width - (xPos * resolutionMultiplier) + (targetXPos * 2 * resolutionMultiplier),
					Screen.height / 2 - (yPos * resolutionMultiplier) + (targetYPos * 2 * resolutionMultiplier),
					HUD_Z)
				);
				break;
			case Position.BottomLeft:
				xPos = (targetXPos + (spriteRenderer.bounds.size.x / 2));
				yPos = targetYPos;
				transform.position = cam.ScreenToWorldPoint(new Vector3(
					xPos * resolutionMultiplier,
					yPos * resolutionMultiplier,
					HUD_Z)
				);
				break;
			case Position.BottomCenter:
				xPos = targetXPos;
				yPos = targetYPos;
				transform.position = cam.ScreenToWorldPoint(new Vector3(
					Screen.width / 2 + (xPos * resolutionMultiplier),
					yPos * resolutionMultiplier,
					HUD_Z)
				);
				break;
			case Position.BottomRight:
				xPos = (targetXPos + (spriteRenderer.bounds.size.x / 2));
				yPos = targetYPos;
				transform.position = cam.ScreenToWorldPoint(new Vector3(
					Screen.width - (xPos * resolutionMultiplier) + (targetXPos * 2 * resolutionMultiplier),
					yPos * resolutionMultiplier,
					HUD_Z)
				);
				break;
			default:
				Assert.IsTrue(false, "** Default Case Reached **");
				break;
		}
	}

	void OnEnable()
	{
		EventKit.Subscribe<bool>("fade hud", OnFadeHud);
	}

	void OnDestroy()
	{
		EventKit.Unsubscribe<bool>("fade hud", OnFadeHud);
	}
}
