using UnityEngine;
using System.Collections;

public class BreakableManager : CacheBehaviour {

    private Sprite[] slices;

	void Start()
    {
        InstantiateBreakablePieces();
	}

    void InstantiateBreakablePieces()
    {
        slices = Resources.LoadAll<Sprite>("Sprites/BreakableCreatures/" + transform.parent.name + "_BREAK");

        GameObject prefab = (GameObject)Resources.Load("Prefabs/Misc/BreakablePiece", typeof(GameObject));

        for (int i = 0; i < slices.Length; i++)
        {
            GameObject newPiece = Object.Instantiate(prefab, transform.position, Quaternion.identity) as GameObject;
            newPiece.transform.parent = gameObject.transform;
            newPiece.name = "Piece_" + i;


            BreakablePiece piece = newPiece.GetComponent<BreakablePiece>();
            piece.Init(slices[i]);
        }

        // foreach(Transform child in transform)
        // {
        //     child.gameObject.GetComponent<BreakablePiece>().SetPosition();
        // }
    }
}
