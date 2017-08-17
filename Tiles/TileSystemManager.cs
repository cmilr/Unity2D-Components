using UnityEngine;
using UnityEngine.Assertions;

public class TileSystemManager : BaseBehaviour
{
	private Shader shader;

	void Start()
	{
		shader = Shader.Find("Sprites/Diffuse");
		Assert.IsNotNull(shader);

		SetChunksToStatic();
		DisableShadows();
	}

	void SetChunksToStatic()
	{
		foreach (Transform child in transform)
			child.gameObject.isStatic = true;
	}

	void DisableShadows()
	{
		MeshRenderer[] allChildren = GetComponentsInChildren<MeshRenderer>();
		Assert.IsNotNull(allChildren);

		foreach (MeshRenderer child in allChildren)
		{
			child.sharedMaterial.shader = shader;
			child.shadowCastingMode     = UnityEngine.Rendering.ShadowCastingMode.Off;
			child.receiveShadows        = false;
			child.lightProbeUsage       = 0;
			child.reflectionProbeUsage  = 0;

			if (debug_TileMapDisabled)
				child.enabled = false;
		}
	}
}
