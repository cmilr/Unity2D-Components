using UnityEngine;

public class TileSystemManager : BaseBehaviour
{
	private Shader shader;

	void Start()
	{
		shader = Shader.Find("Sprites/Diffuse");
		DisableShadows();
	}

	void DisableShadows()
	{
		MeshRenderer[] allChildren = GetComponentsInChildren<MeshRenderer>();

		foreach (MeshRenderer child in allChildren)
		{
			child.sharedMaterial.shader = shader;
			child.shadowCastingMode     = UnityEngine.Rendering.ShadowCastingMode.Off;
			child.receiveShadows        = false;
			child.useLightProbes        = false;
			child.reflectionProbeUsage  = 0;
		}
	}
}
