using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using Matcha.Game.Tweens;


public class DisplayStashed : CacheBehaviour
{
    private int hudSide;
    private float offset;
    private Camera mainCamera;
    private SpriteRenderer HUDWeapon;
    private Vector3 hudPosition;

    void Awake()
    {
        // determine which side of HUD this is attached to, and set paramaters accordingly
        hudSide = (name == "RightWeapon") ? RIGHT : LEFT;
        offset  = (hudSide == RIGHT) ? HUD_STASHED_WEAPON_OFFSET : -HUD_STASHED_WEAPON_OFFSET;
    }

    void Start()
    {
        mainCamera = Camera.main.GetComponent<Camera>();
        Invoke("PositionHUDElements", .001f);
        // HUDWeapon.color = new Color (1f, 1f, 1f, .3f);
    }

    void PositionHUDElements()
    {
        // shift to left or right, depending on which side of HUD we're on
        transform.position = mainCamera.ScreenToWorldPoint(new Vector3(
            Screen.width / 2 + offset,
            Screen.height - HUD_WEAPON_TOP_MARGIN,
            HUD_Z));

        hudPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
    }

    void InitStashedWeapon(GameObject weapon)
    {
        HUDWeapon = spriteRenderer;
        HUDWeapon.sprite = weapon.GetComponent<Weapon>().sprite;
        HUDWeapon.DOKill();
        FadeInWeapon();
    }

    void ChangeStashedWeapon(GameObject weapon)
    {
        HUDWeapon = spriteRenderer;
        HUDWeapon.sprite = weapon.GetComponent<Weapon>().sprite;
        transform.localPosition = hudPosition;

        if (hudSide == RIGHT)
        {
            // upon receiving new weapon for the right slot, instantly relocate it back to its previous location,
            // while setting opacity to 100%, then tween the image to the right, into the final right slot position,
            // all while tweening the transparency down to that of stashed weapons
            MTween.FadeOut(HUDWeapon, 1f, 0, 0);
            MTween.FadeIn(HUDWeapon, HUD_STASHED_TRANSPARENCY, 0f, .2f);

            // shift weapon left, to roughly the position it was just in
            transform.localPosition = new Vector3(
                transform.localPosition.x -1.3281f,
                transform.localPosition.y,
                transform.localPosition.z);

            // tween weapon to new position
            transform.DOLocalMove(new Vector3(
                transform.localPosition.x + SPACE_BETWEEN_WEAPONS,
                transform.localPosition.y,
                transform.localPosition.z), INVENTORY_SHIFT_SPEED, false).OnComplete(()=>SetFinalPosition());
        }
        else
        {
            MTween.FadeOut(HUDWeapon, 0, 0);
            MTween.FadeIn(HUDWeapon, HUD_STASHED_TRANSPARENCY, 0f, .05f);
        }
    }

    void SetFinalPosition()
    {
        // set weapon into final position; fixes graphical bugs if operation gets interrupted by a new click
        // transform.localPosition = hudPosition;
    }

    void FadeInWeapon()
    {
        // fade weapon to zero instantly, then fade up slowly
        MTween.FadeOut(HUDWeapon, 0, 0);
        MTween.FadeIn(HUDWeapon, HUD_STASHED_TRANSPARENCY, HUD_FADE_IN_AFTER, HUD_INITIAL_TIME_TO_FADE);
    }

    void OnFadeHud(bool status)
    {
        MTween.FadeOut(HUDWeapon, HUD_FADE_OUT_AFTER, HUD_INITIAL_TIME_TO_FADE);
    }

    void OnScreenSizeChanged(float vExtent, float hExtent)
    {
        PositionHUDElements();
    }

    void OnInitStashedWeapon(GameObject weapon, int weaponSide)
    {
        if (weaponSide == hudSide)
            InitStashedWeapon(weapon);
    }

    void OnChangeStashedWeapon(GameObject weapon, int weaponSide)
    {
        if (weaponSide == hudSide)
            ChangeStashedWeapon(weapon);
    }

    void OnEnable()
    {
        Messenger.AddListener<GameObject, int>("init stashed weapon", OnInitStashedWeapon);
        Messenger.AddListener<GameObject, int>("change stashed weapon", OnChangeStashedWeapon);
        Messenger.AddListener<bool>("fade hud", OnFadeHud);
        Messenger.AddListener<float, float>( "screen size changed", OnScreenSizeChanged);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener<GameObject, int>("init stashed weapon", OnInitStashedWeapon);
        Messenger.RemoveListener<GameObject, int>("change stashed weapon", OnChangeStashedWeapon);
        Messenger.RemoveListener<bool>("fade hud", OnFadeHud);
        Messenger.RemoveListener<float, float>( "screen size changed", OnScreenSizeChanged);
    }
}
