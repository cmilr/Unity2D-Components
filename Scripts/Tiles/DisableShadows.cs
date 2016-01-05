using UnityEngine;

public class DisableShadows : BaseBehaviour
{
	private Shader shader;

	void Start()
	{
		MeshRenderer[] allChildren = GetComponentsInChildren<MeshRenderer>();
		shader = Shader.Find("Sprites/Diffuse");

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
