using UnityEngine;
using System.Collections;

public class WaterEntity : Entity 
{
	public override void ReactToCollision()
	{
		Debug.Log("WaterEntity does not currently have a reaction.");
	}

}
