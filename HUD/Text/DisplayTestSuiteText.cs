using UnityEngine.Assertions;

public class DisplayTestSuiteText : HudTextBehaviour
{
	void Awake()
	{
		legend 		= "TEST #";
		targetXPos 	= 0f;
		targetYPos 	= 0f;

		CalculatePosition();
		BaseAwake();
	}

	void Start()
	{
		BaseStart();
		BaseOnInitInteger(10);
	}

	void CalculatePosition()
	{
		switch (name)
		{
			case "Test_1":
				anchorPosition = Position.TopLeft;
				break;
			case "Test_2":
				anchorPosition = Position.TopCenter;
				break;
			case "Test_3":
				anchorPosition = Position.TopRight;
				break;
			case "Test_4":
				anchorPosition = Position.MiddleLeft;
				break;
			case "Test_5":
				anchorPosition = Position.MiddleCenter;
				break;
			case "Test_6":
				anchorPosition = Position.MiddleRight;
				break;
			case "Test_7":
				anchorPosition = Position.BottomLeft;
				break;
			case "Test_8":
				anchorPosition = Position.BottomCenter;
				break;
			case "Test_9":
				anchorPosition = Position.BottomRight;
				break;
			default:
				Assert.IsTrue(false, "Default case reached @ " + gameObject);
				break;
		}
	}
}
