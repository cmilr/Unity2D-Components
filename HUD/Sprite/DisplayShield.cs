
public class DisplayShield : HudSpriteBehaviour
{
	void Awake()
	{
		anchorPosition 	= INVENTORY_ALIGNMENT;
		targetXPos 		= INVENTORY_X_POS;
		targetYPos 		= INVENTORY_Y_POS;

		BaseAwake();
	}
	
	void Start()
	{
		BaseStart();
	}
}
