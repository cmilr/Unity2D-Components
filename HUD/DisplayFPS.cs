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
		
		canvasScaler.scaleFactor = PLATFORM_SPECIFIC_CANVAS_SCALE;
		M.PositionInHUD(rectTrans, text, FPS_ALIGNMENT, FPS_X_POS, FPS_Y_POS);
	}
}
