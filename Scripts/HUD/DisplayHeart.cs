using DG.Tweening;
using Matcha.Dreadful;
using UnityEngine;
using UnityEngine.Assertions;

public class DisplayHeart : BaseBehaviour
{
    public Sprite heartSprite;
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
        (fadeOutHUD     = MFX.Fade(spriteRenderer, 0, HUD_FADE_OUT_AFTER, HUD_INITIAL_TIME_TO_FADE)).Pause();
        (fadeOutInstant = MFX.Fade(spriteRenderer, 0, 0, 0)).Pause();
        (fadeIn         = MFX.Fade(spriteRenderer, 1, HUD_FADE_IN_AFTER, HUD_INITIAL_TIME_TO_FADE)).Pause();

        spriteRenderer.sprite = heartSprite;

        PositionHUDElements();
        FadeInShield();
    }

    void PositionHUDElements()
    {
        switch (name)
        {
            case "Heart_1":
                transform.position = mainCamera.ScreenToWorldPoint(new Vector3(
                    Screen.width  - (HEART_X_POS * RESOLUTION_OFFSET),
                    Screen.height - (HEART_Y_POS * RESOLUTION_OFFSET),
                    HUD_Z)
                );
                break;
            case "Heart_2":
                transform.position = mainCamera.ScreenToWorldPoint(new Vector3(
                    Screen.width  - (HEART_X_POS * RESOLUTION_OFFSET) - (HEART_OFFSET * RESOLUTION_OFFSET),
                    Screen.height - (HEART_Y_POS * RESOLUTION_OFFSET),
                    HUD_Z)
                );
                break;
            case "Heart_3":
                transform.position = mainCamera.ScreenToWorldPoint(new Vector3(
                    Screen.width  - (HEART_X_POS * RESOLUTION_OFFSET) - (HEART_OFFSET * RESOLUTION_OFFSET * 2),
                    Screen.height - (HEART_Y_POS * RESOLUTION_OFFSET),
                    HUD_Z)
                );
                break;
            default:
                Assert.IsTrue(false, "Default case reached @ " + gameObject);
                break;
        }
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
