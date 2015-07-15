using UnityEngine;


public class Tile : BaseBehaviour
{
    // TileType is used to block enemy AI from walking off into space, etc
    public enum TileType { Decorative, Solid, OneWay };
    public TileType tileType = TileType.Decorative;
    private Shader shader1;
    private MeshRenderer meshRenderer;

    void Start() {

        shader1                        = Shader.Find("Sprites/Diffuse");
        meshRenderer                   = transform.parent.GetComponent<MeshRenderer>();
        meshRenderer.material.shader   = shader1;
        meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        meshRenderer.receiveShadows    = false;
    }
}




// void Start()
// {
//  // customized tiles with colliders, etc, need to have their parent gameObject's
//  // Mesh Renderer turned off, to expose the prefab sprite renderer underneath.
//  // this allows use of the Diffusion Sprite Renderer, for reflective light, etc.

//  // if (gameObject.name != DEFAULT_TILE)
//  //  transform.parent.GetComponent<MeshRenderer>().enabled = false;
// }