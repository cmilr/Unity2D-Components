using DG.Tweening;
using Matcha.Dreadful;
using Matcha.Unity;
using UnityEngine;
using UnityEngine.Assertions;

public class DisplayStashedRight : HudSpriteBehaviour
{
	private Vector3 slideTweenOrigin;
	private Sequence fadeToTint;
	private Sequence fadeInInstant;
	private Tween slideItem;
	private Tween setTint;

	void Awake()
	{
		anchorPosition 	= INVENTORY_ALIGNMENT;
		targetXPos 		= INVENTORY_X_POS + STASHED_ITEM_OFFSET;
		targetYPos 		= INVENTORY_Y_POS;

		BaseAwake();
	}
	
	void Start()
	{
		// cache & pause tween sequences.
		(setTint 			= MFX.SetToStashedTint(spriteRenderer)).Pause();
		(fadeToTint 		= MFX.FadeToStashedTint(spriteRenderer, 0f, .2f)).Pause();
		(fadeInInstant      = MFX.Fade(spriteRenderer, 1, 0, 0)).Pause();

		Invoke("SetTweens", .001f);
		BaseStart();
	}

	void SetTweens()
	{
		// set origin for item's slide tween
		slideTweenOrigin = new Vector3(
			transform.localPosition.x - STASHED_ITEM_OFFSET,
			transform.localPosition.y,
			transform.localPosition.z
		);

		// cache & pause slide tween.
		(slideItem = MFX.SlideStashedItem(transform, slideTweenOrigin, STASHED_ITEM_OFFSET, INVENTORY_SHIFT_SPEED, false)).Pause();
	}

	void InitStashedWeapon(GameObject weapon)
	{
		spriteRenderer.sprite = weapon.GetComponent<Weapon>().iconSprite;
		fadeOutInstant.Restart();
		setTint.Restart();
		fadeInHUD.Restart();
	}

	void ChangeStashedWeapon(GameObject weapon)
	{
		spriteRenderer.sprite = weapon.GetComponent<Weapon>().iconSprite;
		Assert.IsNotNull(spriteRenderer.sprite);

		// upon receiving new weapon, instantly relocate it back to its previous location.
		transform.localPosition = new Vector3(
			slideTweenOrigin.x,
			slideTweenOrigin.y,
			slideTweenOrigin.z
		);

		// set opacity to 100%. 
		fadeInInstant.Restart();
		// then tween the transparency down to that of stashed weapons.
		fadeToTint.Restart();
		// finally, tween the image to the right, into its final slot position.
		slideItem.Restart();
	}

	void OnInitStashedWeapon(GameObject weapon, Side weaponSide)
	{
		if (weaponSide == Side.Right)
		{
			InitStashedWeapon(weapon);
		}
	}

	void OnChangeStashedWeapon(GameObject weapon, Side weaponSide)
	{
		if (weaponSide == Side.Right)
		{
			ChangeStashedWeapon(weapon);
		}
	}

	void OnEnable()
	{
		EventKit.Subscribe<GameObject, Side>("init stashed weapon", OnInitStashedWeapon);
		EventKit.Subscribe<GameObject, Side>("change stashed weapon", OnChangeStashedWeapon);
		EventKit.Subscribe("reposition hud elements", SetTweens);
	}

	void OnDestroy()
	{
		EventKit.Unsubscribe<GameObject, Side>("init stashed weapon", OnInitStashedWeapon);
		EventKit.Unsubscribe<GameObject, Side>("change stashed weapon", OnChangeStashedWeapon);
		EventKit.Unsubscribe("reposition hud elements", SetTweens);
	}
}
