using DG.Tweening;
using Matcha.Dreadful;
using UnityEngine;
using UnityEngine.Assertions;

public class DisplayStashedLeft : HudSpriteBehaviour
{
	private Vector3 slideTweenOrigin;
	private Sequence fadeToTint;
	private Tween slideItem;
	private Tween setTint;

	void Awake()
	{
		anchorPosition 	= INVENTORY_ALIGNMENT;
		targetXPos 		= INVENTORY_X_POS - STASHED_ITEM_OFFSET;
		targetYPos 		= INVENTORY_Y_POS;

		BaseAwake();
	}

	void Start()
	{
		// cache & pause tween sequences.
		(setTint 			= MFX.SetToStashedTint(spriteRenderer)).Pause();
		(fadeToTint 		= MFX.FadeToStashedTint(spriteRenderer, 0f, .05f)).Pause();
		(fadeInHUD 			= MFX.Fade(spriteRenderer, 1, HUD_FADE_IN_AFTER, HUD_INITIAL_FADE_LENGTH)).Pause();
		(fadeOutHUD 		= MFX.Fade(spriteRenderer, 0, HUD_FADE_OUT_AFTER, HUD_INITIAL_FADE_LENGTH)).Pause();
		(fadeOutInstant		= MFX.Fade(spriteRenderer, 0, 0, 0)).Pause();

		BaseStart();
	}

	void InitStashedWeapon(GameObject weapon)
	{
		spriteRenderer.sprite = weapon.GetComponent<Weapon>().iconSprite;
		Assert.IsNotNull(spriteRenderer.sprite);

		fadeOutInstant.Restart();
		setTint.Restart();
		fadeInHUD.Restart();
	}

	void ChangeStashedWeapon(GameObject weapon)
	{
		spriteRenderer.sprite = weapon.GetComponent<Weapon>().iconSprite;
		Assert.IsNotNull(spriteRenderer.sprite);

		fadeOutInstant.Restart();
		fadeToTint.Restart();
	}

	void OnInitStashedWeapon(GameObject weapon, Side weaponSide)
	{
		if (weaponSide == Side.Left) {
			InitStashedWeapon(weapon);
		}
	}

	void OnChangeStashedWeapon(GameObject weapon, Side weaponSide)
	{
		if (weaponSide == Side.Left) {
			ChangeStashedWeapon(weapon);
		}
	}

	void OnEnable()
	{
		EventKit.Subscribe<GameObject, Side>("init stashed weapon", OnInitStashedWeapon);
		EventKit.Subscribe<GameObject, Side>("change stashed weapon", OnChangeStashedWeapon);
	}

	void OnDestroy()
	{
		EventKit.Unsubscribe<GameObject, Side>("init stashed weapon", OnInitStashedWeapon);
		EventKit.Unsubscribe<GameObject, Side>("change stashed weapon", OnChangeStashedWeapon);
	}
}
