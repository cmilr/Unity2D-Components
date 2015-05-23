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

    void Awake()
    {
        // check which HUD side this is, and set paramaters accordingly
        if (name == "RightWeapon")
        {
            hudSide = RIGHT;
            offset  = HUD_STASHED_WEAPON_OFFSET;
        }
        else if (name == "LeftWeapon")
        {
            hudSide = LEFT;
            offset  = -HUD_STASHED_WEAPON_OFFSET;
        }
    }

    void Start()
    {
        mainCamera = Camera.main.GetComponent<Camera>();
        Invoke("PositionHUDElements", .001f);
        // HUDWeapon.color = new Color (1f, 1f, 1f, .3f);
    }

    void PositionHUDElements()
    {
        // shift to left or right, depending on which GameObject this is attached to
        transform.position = mainCamera.ScreenToWorldPoint(new Vector3(
            Screen.width / 2 + offset,
            Screen.height - HUD_WEAPON_TOP_MARGIN,
            HUD_Z));
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
        HUDWeapon.DOKill();
        MTween.FadeOut(HUDWeapon, 0, 0);
        MTween.FadeIn(HUDWeapon, HUD_STASHED_TRANSPARENCY, 0f, HUD_WEAPON_CHANGE_FADE);
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
