using UnityEngine;


public class TileEntity : BaseBehaviour
{	
	void Start()
	{
		// customized tiles with colliders, etc, need to have their parent gameObject's
		// Mesh Renderer turned off, to expose the prefab sprite renderer underneath.
		// this allows the use of Diffusion Sprite Renderer, for reflective light, etc.
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
