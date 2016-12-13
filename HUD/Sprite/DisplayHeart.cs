using UnityEngine.Assertions;

public class DisplayHeart : HudSpriteBehaviour
{
	void Awake()
	{
		anchorPosition 	= HEART_ALIGNMENT;
		targetXPos 		= HEART_X_POS;
		targetYPos 		= HEART_Y_POS;

		CalculatePositions();

		BaseAwake();
	}

	void Start() 
	{ 
		BaseStart(); 
	}

	void CalculatePositions()
	{
		switch (name)
		{
			case "Heart_1":
				break;
			case "Heart_2":
				targetXPos = HEART_X_POS - HEART_OFFSET;
				break;
			case "Heart_3":
				targetXPos = HEART_X_POS - (HEART_OFFSET * 2);
				break;
			default:
				Assert.IsTrue(false, "Default case reached @ " + gameObject);
				break;
		}
	}
}
