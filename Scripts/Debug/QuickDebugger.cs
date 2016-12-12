using UnityEngine;
using UnityEngine.Assertions;
using Rotorz.Tile;

public class QuickDebugger : BaseBehaviour
{
	[Header("QUICK ENABLE/DISABLE")]
	public bool vSyncEnabled;
	public bool attackEnabled;
	public bool movementEnabled;
	public bool tileMapEnabled;

	private bool tileMapCurrentlyDisabled;
	private GameObject tileMap;

	void Start()
	{
		InvokeRepeating("SlowUpdate", 0f, .2f);
	}

	void SlowUpdate()
	{
		QualitySettings.vSyncCount = vSyncEnabled ? 1 : 0;
		debug_AttackDisabled = !attackEnabled;
		debug_MovementDisabled = !movementEnabled;
		CheckTileMapStatus();
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
}
