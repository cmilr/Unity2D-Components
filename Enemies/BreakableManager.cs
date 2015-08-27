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
        // find and load the sprite for this particular creature
        slices = Resources.LoadAll<Sprite>("Sprites/BreakableCreatures/" + transform.parent.name + "_BREAK");

        // find and load the 'BreakablePiece' prefab
        GameObject prefab = (GameObject)Resources.Load("Prefabs/Misc/BreakablePiece", typeof(GameObject));

        // instantiate the prefab as many times as required via this loop
        for (int i = 0; i < slices.Length; i++)
        {
            GameObject newPiece = Object.Instantiate(prefab, transform.position, Quaternion.identity) as GameObject;
            newPiece.transform.parent = gameObject.transform;
            newPiece.GetComponent<BreakablePiece>().Init(i, slices[i]);
        }

        // deactivate after a delay, so other components have time to cache a reference
        Invoke("MakeInactive", .5f);
    }

    void MakeInactive()
    {
        gameObject.SetActive(false);
    }

    public void Explode()
    {
        gameObject.SetActive(true);

        foreach (Transform child in transform)
        {
            Rigidbody2D piece = child.GetComponent<Rigidbody2D>();
            piece.isKinematic = false;
            piece.AddForce(new Vector3(50f, 50f, 0f), ForceMode2D.Impulse);
        }
    }
}
