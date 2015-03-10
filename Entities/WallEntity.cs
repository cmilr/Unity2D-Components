using UnityEngine;
using System.Collections;

public class WallEntity : Entity 
{
	override public void OnBodyCollisionEnter()
	{
		Messenger.Broadcast<bool>("touching wall", true);
	}

	override public void OnBodyCollisionStay()
	{
		Messenger.Broadcast<bool>("touching wall", true);
	}

	override public void OnBodyCollisionExit()
	{	
		Messenger.Broadcast<bool>("touching wall", false);
	}
}
