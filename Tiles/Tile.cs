using UnityEngine;


public class Tile : BaseBehaviour
{

	void Start()
	{
		// customized tiles with colliders, etc, need to have their parent gameObject's
		// Mesh Renderer turned off, to expose the prefab sprite renderer underneath.
		// this allows use of the Diffusion Sprite Renderer, for reflective light, etc.

		if (gameObject.name != DEFAULT_TILE)
			transform.parent.GetComponent<MeshRenderer>().enabled = false;
	}
}
