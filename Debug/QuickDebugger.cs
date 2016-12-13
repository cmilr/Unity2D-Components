using UnityEngine;
using UnityEngine.Assertions;

public class QuickDebugger : BaseBehaviour
{
	#if UNITY_EDITOR

	[Header("QUICK ENABLE/DISABLE")]
	public bool vSyncEnabled;
	public bool attackEnabled;
	public bool movementEnabled;
	public bool tileMapEnabled;
	public bool lockPlayer;

	[Header("QUICK CAMERA ADJUST")]
	public int horizUnitsOnScreen;

	private int actualUnitsOnScreen;
	private int savedUnitsOnScreen;
	private bool tileMapCurrentlyDisabled;
	private GameObject tileMap;
	private Transform player;
	private Vector3 startingPosition;

	void Start()
	{
		player = GameObject.Find(PLAYER).transform;
		Assert.IsNotNull(player);

		actualUnitsOnScreen = Camera.main.GetComponent<PixelArtCamera>().VertUnitsOnScreen;
		Assert.AreNotApproximatelyEqual(0.0f, actualUnitsOnScreen);

		horizUnitsOnScreen = savedUnitsOnScreen = actualUnitsOnScreen;

		startingPosition = player.position;
			
		InvokeRepeating("PeriodicUpdate", 0f, .2f);
	}

	void PeriodicUpdate()
	{
		QualitySettings.vSyncCount = vSyncEnabled ? 1 : 0;
		debug_AttackDisabled = !attackEnabled;
		debug_MovementDisabled = !movementEnabled;

		CheckTileMapStatus();

		if (horizUnitsOnScreen != actualUnitsOnScreen)
		{
			savedUnitsOnScreen = actualUnitsOnScreen;

			EventKit.Broadcast("set screen size", horizUnitsOnScreen);
		}
	}

	void LateUpdate()
	{
		if (lockPlayer)
		{
			player.position = startingPosition;
		}
	}

	void CheckTileMapStatus()
	{
		if (!tileMapCurrentlyDisabled && !tileMapEnabled)
		{
			DisableTileMapRenderers(true);
			tileMapCurrentlyDisabled = true;
		}
		else if (tileMapCurrentlyDisabled && tileMapEnabled)
		{
			DisableTileMapRenderers(false);
			tileMapCurrentlyDisabled = false;
		}
	}

	void DisableTileMapRenderers(bool status)
	{
		tileMap = GameObject.Find(TILE_MAP);
		Assert.IsNotNull(tileMap);

		MeshRenderer[] allChildren = tileMap.GetComponentsInChildren<MeshRenderer>();
		Assert.IsNotNull(allChildren);

		foreach (MeshRenderer child in allChildren)
		{
			child.enabled = !status;
		}
	}

	void OnApplicationQuit()
	{
		EventKit.Broadcast("set screen size", savedUnitsOnScreen);
	}

	void OnDisable()
	{
		EventKit.Broadcast("reposition hud elements");

		UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
	}

	#endif
}
