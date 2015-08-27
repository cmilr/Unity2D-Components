using UnityEngine;
using System.Collections;
using Matcha.Extensions;

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

    public void Explode(int hitFrom)
    {
        gameObject.SetActive(true);

        // randomly choose an explosion type
        int explosionType;

        switch (UnityEngine.Random.Range(0, 2))
        {
            case 0:
                explosionType = EXPLOSION;
            break;

            case 1:
                explosionType = DIRECTIONAL_EXPLOSION;
            break;

            default:
                explosionType = EXPLOSION;
            break;
        }

        // cycle through pieces and send them flying
        foreach (Transform child in transform)
        {
            Rigidbody2D piece = child.GetComponent<Rigidbody2D>();
            piece.isKinematic = false;

            switch (explosionType)
            {
                case EXPLOSION:
                    piece.AddExplosionForce(2000, transform.position, 20);
                break;

                case DIRECTIONAL_EXPLOSION:
                    int force = (hitFrom == RIGHT) ? -50 : 50;
                    piece.AddForce(new Vector3(force, 50, 50), ForceMode2D.Impulse);
                break;

                default:
                    piece.AddExplosionForce(2000, transform.position, 5);
                break;
            }
        }
    }
}





