using UnityEngine;
using System.Collections;

public class TileMapManager : BaseBehaviour {

	public float groundLine = -50.00f;

	void Start () 
	{
		Messenger.Broadcast<float>("set ground line", groundLine);
	}
}
