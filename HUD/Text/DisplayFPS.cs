using Matcha.Unity;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class DisplayFPS : BaseBehaviour
{
	private Text textComponent;
	private CanvasScaler canvasScaler;
	private RectTransform rectTrans;
	private Text text;
	private float canvasScale;

	void Awake()
	{
		if (Debug.isDebugBuild)
		{
			rectTrans = GetComponent<RectTransform>();
			Assert.IsNotNull(rectTrans);

			text = GetComponent<Text>();
			Assert.IsNotNull(text);
		}
		else
		{
			gameObject.SetActive(false);
		}
	}
	
	void Start()
	{
		canvasScaler = gameObject.GetComponentInParent<CanvasScaler>();
		Assert.IsNotNull(canvasScaler);

		canvasScale = Camera.main.GetComponent<PixelArtCamera>().CanvasScale;
		Assert.AreNotApproximatelyEqual(0.0f, canvasScale);

		PositionHUDElements();
	}

	void PositionHUDElements()
	{
		canvasScaler.scaleFactor = canvasScale;
		M.PositionInHUD(rectTrans, text, FPS_ALIGNMENT, FPS_X_POS, FPS_Y_POS);
	}

	void OnEnable()
	{
		EventKit.Subscribe("reposition hud elements", PositionHUDElements);
	}

	void OnDestroy()
	{
		EventKit.Unsubscribe("reposition hud elements", PositionHUDElements);
	}
}
