
public class DisplayWaterDrop : HudSpriteBehaviour
{
	void Awake()
	{
		anchorPosition	= WATER_DROP_ALIGNMENT;
		targetXPos		= WATER_DROP_X_POS;
		targetYPos		= WATER_DROP_Y_POS;

		BaseAwake();
	}

	void Start()
	{
		BaseStart();
	}
}
