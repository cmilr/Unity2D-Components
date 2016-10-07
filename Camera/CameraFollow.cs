using Rotorz.Tile;
using UnityEngine;
using UnityEngine.Assertions;

public class CameraFollow : BaseBehaviour
{
	// margin and speed settings have been migrated to
	// BaseBehaviour.cs, for ease of cross-platform setup.

	private float camSmooth = .04f;
	private float tileSysRightBound;
	private float tileSysLefBound;
	private float tileSysTopBound;
	private float tileSysBottomBound;
	private float vertExtent;
	private float horizExtent;
	private Vector3 tileSystemSize;
	private TileSystem tileSystem;
	private Transform player;
	private new Transform transform;
	
	//unused Vector3 for SmoothDamp function.
	private Vector3 velocity = Vector3.zero;

	void Awake()
	{
		transform = GetComponent<Transform>();
		Assert.IsNotNull(transform);
	}
	
	void Start()
	{		
		player = GameObject.Find(PLAYER).GetComponent<Transform>();
		Assert.IsNotNull(player);
		
		tileSystem = GameObject.Find(TILE_MAP).GetComponent<TileSystem>();
		Assert.IsNotNull(tileSystem);

		CalculateExtents();
		CalculateTileSystemSize();
		CalculateScreenBounds();
		CalculateStartingPosition();
	}

	void CalculateExtents()
	{
		vertExtent  = Camera.main.GetComponent<Camera>().orthographicSize;
		horizExtent = vertExtent * Screen.width / Screen.height;
	}

	void CalculateTileSystemSize()
	{
		tileSystemSize = new Vector3(
			tileSystem.ColumnCount * tileSystem.CellSize.x,
			tileSystem.RowCount * tileSystem.CellSize.y,
			tileSystem.CellSize.z
		);
	}

	void CalculateScreenBounds()
	{
		tileSysLefBound   = (horizExtent);
		tileSysRightBound  = (tileSystemSize.x - horizExtent);
		tileSysBottomBound = (-tileSystemSize.y + vertExtent);
		tileSysTopBound    = (vertExtent);
	}

	void CalculateStartingPosition()
	{
		transform.SetPositionXY(
			player.position.x - CAM_STARTING_X_POSITION,
			player.position.y - CAM_STARTING_Y_POSITION + vertExtent
		);
	}

	void LateUpdate()
	{
		TrackPlayer();
	}

	void TrackPlayer()
	{
		float targetX = transform.position.x;
		float targetY = transform.position.y;

		//follow player once he moves beyond xMargin.
		if (PlayerAtXMargin()) {
			targetX = Mathf.Lerp(transform.position.x, player.position.x, CAM_X_SPEED_TO_FOLLOW * Time.deltaTime);
		}

		//keep a margin between player and bottom of screen
		if (DistanceFromEdge(BOTTOM) <= MIN_BOTTOM_SCREEN_MARGIN) {
			targetY = player.position.y - MIN_BOTTOM_SCREEN_MARGIN + vertExtent;
		}
		//keep a margin between player and top of screen,
		else if (DistanceFromEdge(TOP) <= MIN_TOP_SCREEN_MARGIN)
		{
			targetY = Mathf.Lerp(transform.position.y, player.position.y + MIN_TOP_SCREEN_MARGIN - vertExtent, CAM_Y_SPEED_TO_FOLLOW * Time.deltaTime);
		}

		//keep camera from leaving the tileSystem.
		targetX = Mathf.Clamp(targetX, tileSysLefBound, tileSysRightBound);
		targetY = Mathf.Clamp(targetY, tileSysBottomBound, tileSysTopBound);

		//move camera.
		transform.position = Vector3.SmoothDamp(transform.position,
						new Vector3(targetX, targetY, transform.position.z), ref velocity, camSmooth);
	}

	bool PlayerAtXMargin()
	{
		return Mathf.Abs(transform.position.x - player.position.x) > PLAYER_X_MOVEMENT_BEFORE_CAM_FOLLOWS;
	}

	//returns the distance from a gameObject to the edge of the screen on 2D orthographic cameras.
	float DistanceFromEdge(int screenEdge)
	{
		switch (screenEdge)
		{
			case TOP:
				return Mathf.Abs(transform.position.y + vertExtent - player.position.y);
			case BOTTOM:
				return Mathf.Abs(transform.position.y - vertExtent - player.position.y);
			case LEFT:
				return Mathf.Abs(transform.position.x - horizExtent - player.position.x);
			case RIGHT:
				return Mathf.Abs(transform.position.x + horizExtent - player.position.x);
			default:
				Assert.IsTrue(false, "** Default Case Reached **");
				return ERROR;
		}
	}

	void OnScreenSizeChanged(float vExtent, float hExtent)
	{
		CalculateExtents();
		CalculateScreenBounds();
	}

	void OnPlayerDead(Hit incomingHit)
	{
		camSmooth = .07f;
	}

	void OnEnable()
	{
		Start();
		EventKit.Subscribe<Hit>("player dead", OnPlayerDead);
		EventKit.Subscribe<float, float>("screen size changed", OnScreenSizeChanged);
	}

	void OnDestroy()
	{
		EventKit.Unsubscribe<Hit>("player dead", OnPlayerDead);
		EventKit.Unsubscribe<float, float>("screen size changed", OnScreenSizeChanged);
	}
}
