using UnityEngine;


public class TileEntity : BaseBehaviour
{	
	void Start()
	{
		//
		if (gameObject.name != "DefaultTile")
			transform.parent.GetComponent<MeshRenderer>().enabled = false;
	}

	// void OnBecameVisible()
	// {
	// 	collider2D.enabled = true;
	// }

	// void OnBecameInvisible()
	// {
	// 	collider2D.enabled = false;
	// }
}
