using DG.Tweening;
using Matcha.Dreadful;
using UnityEngine;

public class DisplayEquipped : HudSpriteBehaviour
{
	private Vector3 slideTweenOrigin;
	private Tween slideItem;
	private Tween fadeToNoTint;
	private Tween setToStashedTint;

	void Awake()
	{
		anchorPosition 	= INVENTORY_ALIGNMENT;
		targetXPos 		= INVENTORY_X_POS;
		targetYPos 		= INVENTORY_Y_POS;

		BaseAwake();
	}
	
	void Start()
	{
		// cache & pause tween sequences.
		(fadeToNoTint       = MFX.FadeToNoTint(spriteRenderer, 0, .5f)).Pause();
		(setToStashedTint 	= MFX.SetToStashedTint(spriteRenderer)).Pause();

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

	void OnInitWeapon(GameObject weapon)
	{
		spriteRenderer.sprite = weapon.GetComponent<Weapon>().iconSprite;
		fadeOutInstant.Restart();
		fadeInHUD.Restart();
	}
	
	void OnInitNewWeapon(GameObject weapon)
	{
		spriteRenderer.sprite = weapon.GetComponent<Weapon>().iconSprite;
	}

	void OnChangeWeapon(GameObject weapon)
	{
		spriteRenderer.sprite = weapon.GetComponent<Weapon>().iconSprite;

		// upon receiving new weapon, instantly relocate it back to its previous location.
		transform.localPosition = new Vector3(
			slideTweenOrigin.x,
			slideTweenOrigin.y,
			slideTweenOrigin.z
		);

		// set tint to that of stashed weapons. 
		setToStashedTint.Restart();
		// then tween to NO tint.
		fadeToNoTint.Restart();
		// finally, tween the image to the right, into its final slot position.
		slideItem.Restart();
	}

	void OnEnable()
	{
		EventKit.Subscribe<GameObject>("init equipped weapon", OnInitWeapon);
		EventKit.Subscribe<GameObject>("init new equipped weapon", OnInitNewWeapon);
		EventKit.Subscribe<GameObject>("change equipped weapon", OnChangeWeapon);
		EventKit.Subscribe("reposition hud elements", SetTweens);
	}

	void OnDestroy()
	{
		EventKit.Unsubscribe<GameObject>("init equipped weapon", OnInitWeapon);
		EventKit.Unsubscribe<GameObject>("init new equipped weapon", OnInitNewWeapon);
		EventKit.Unsubscribe<GameObject>("change equipped weapon", OnChangeWeapon);
		EventKit.Unsubscribe("reposition hud elements", SetTweens);
	}
}
