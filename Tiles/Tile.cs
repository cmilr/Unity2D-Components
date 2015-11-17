using UnityEngine;

public class Tile : BaseBehaviour
{
	// TileType is used to block enemy AI from walking off into space, etc
	public enum TileType { Decorative, Solid, OneWay };
	public TileType tileType = TileType.Decorative;
	private Shader shader1;
	private MeshRenderer meshRenderer;

	void Start()
	{
		// change the default tile specs given by Rotorz
		shader1                            = Shader.Find("Sprites/Diffuse");
		meshRenderer                       = transform.parent.GetComponent<MeshRenderer>();
		meshRenderer.sharedMaterial.shader = shader1;
		meshRenderer.shadowCastingMode     = UnityEngine.Rendering.ShadowCastingMode.Off;
		meshRenderer.receiveShadows        = false;
		meshRenderer.useLightProbes        = false;
	}
}
