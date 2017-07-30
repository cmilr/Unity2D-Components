
public class DisplayLvl : HudTextBehaviour
{
	void Awake()
	{
		legend 			= "Lvl: ";
		anchorPosition 	= LEVEL_ALIGNMENT;
		targetXPos 		= LEVEL_X_POS;
		targetYPos 		= LEVEL_Y_POS;

		BaseAwake();
	}

	void Start()
	{
		BaseStart();
	}

	void OnInitInteger(int newInt)
	{
		BaseOnInitInteger(newInt);
	}

	void OnEnable()
	{
		EventKit.Subscribe<int>("init lvl", OnInitInteger);
	}

	void OnDestroy()
	{
		EventKit.Unsubscribe<int>("init lvl", OnInitInteger);
	}
}
