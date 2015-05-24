using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using Matcha.Game.Tweens;


public class DisplayEquipped : CacheBehaviour
{
    private SpriteRenderer HUDWeapon;
    private Camera mainCamera;
    private Vector3 hudPosition;

    void Start()
    {
        mainCamera = Camera.main.GetComponent<Camera>();
        PositionHUDElements();
    }

    void PositionHUDElements()
    {
        transform.position = mainCamera.ScreenToWorldPoint(new Vector3(
            Screen.width / 2,
            Screen.height - HUD_WEAPON_TOP_MARGIN,
            HUD_Z));

        hudPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
    }

    void OnInitEquippedWeapon(GameObject weapon)
    {
        HUDWeapon = spriteRenderer;
        HUDWeapon.sprite = weapon.GetComponent<Weapon>().sprite;
        HUDWeapon.DOKill();
        FadeInWeapon();
    }

    void OnChangeEquippedWeapon(GameObject weapon)
    {
        HUDWeapon = spriteRenderer;
        HUDWeapon.sprite = weapon.GetComponent<Weapon>().sprite;
        HUDWeapon.DOKill();

        // upon receiving new weapon for the equipped slot, instantly relocate it back to its previous location,
        // while setting opacity to that of a stashed weapon, then tween the image to the right, into the final
        // equipped slot position, all while tweening the opacity back to 100%
        MTween.FadeOut(HUDWeapon, HUD_STASHED_TRANSPARENCY, 0, 0);
        MTween.FadeIn(HUDWeapon, 1f, 0f, .2f);

        // shift weapon left, to roughly the position it was just in
        transform.localPosition = new Vector3(
            transform.localPosition.x -1.1f,
            transform.localPosition.y,
            transform.localPosition.z);

        // tween weapon to new position
        transform.DOLocalMove(new Vector3(
            transform.localPosition.x + 1.1f,
            transform.localPosition.y,
            transform.localPosition.z), .1f, false).OnComplete(()=>SetFinalPosition());
    }

    void SetFinalPosition()
    {
        // set weapon into final position; fixes graphical bugs is operation gets interrupted by a new click
        transform.localPosition = hudPosition;
    }

    void FadeInWeapon()
    {
        // fade weapon to zero instantly, then fade up slowly
        MTween.FadeOut(HUDWeapon, 0, 0);
        MTween.FadeIn(HUDWeapon, HUD_FADE_IN_AFTER, HUD_INITIAL_TIME_TO_FADE);
    }

    void OnFadeHud(bool status)
    {
        MTween.FadeOut(HUDWeapon, HUD_FADE_OUT_AFTER, HUD_INITIAL_TIME_TO_FADE);
    }

    void OnScreenSizeChanged(float vExtent, float hExtent)
    {
        PositionHUDElements();
    }

    void OnEnable()
    {
        Messenger.AddListener<GameObject>("init equipped weapon", OnInitEquippedWeapon);
        Messenger.AddListener<GameObject>("change equipped weapon", OnChangeEquippedWeapon);
        Messenger.AddListener<bool>("fade hud", OnFadeHud);
        Messenger.AddListener<float, float>( "screen size changed", OnScreenSizeChanged);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener<GameObject>("init equipped weapon", OnInitEquippedWeapon);
        Messenger.RemoveListener<GameObject>("change equipped weapon", OnChangeEquippedWeapon);
        Messenger.RemoveListener<bool>("fade hud", OnFadeHud);
        Messenger.RemoveListener<float, float>( "screen size changed", OnScreenSizeChanged);
    }
}
