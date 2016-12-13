
public class DisplayHP : HudTextBehaviour
{
	void Awake()
	{
		legend 			= "HP: ";
		anchorPosition 	= HP_ALIGNMENT;
		targetXPos 		= HP_X_POS;
		targetYPos 		= HP_Y_POS;

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
		EventKit.Subscribe<int>("init hp", OnInitInteger);
		EventKit.Subscribe<int>("reduce hp", OnInitInteger);
	}

	void OnDestroy()
	{
		EventKit.Unsubscribe<int>("init hp", OnInitInteger);
		EventKit.Unsubscribe<int>("reduce hp", OnInitInteger);
	}
}
