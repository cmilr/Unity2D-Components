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

    // types of explosions for the Explode() function
    enum Type { Explosion, Directional_Explosion, Slump, Directional_Slump, Geyser };
    Type explosion;

    public void Explode(int hitFrom)
    {
        gameObject.SetActive(true);

        // randomly choose an explosion type
        switch (UnityEngine.Random.Range(1, 2))
        {
            case 0:
                explosion = Type.Explosion;
            break;

            case 1:
                explosion = Type.Directional_Explosion;
            break;

            case 2:
                explosion = Type.Slump;
            break;

            case 3:
                explosion = Type.Directional_Slump;
            break;

            case 4:
                explosion = Type.Geyser;
            break;

            default:
                explosion = Type.Slump;
            break;
        }

        // cycle through pieces and send them flying
        foreach (Transform child in transform)
        {
            // activate physics on this piece
            Rigidbody2D rigidbody2D = child.GetComponent<Rigidbody2D>();
            rigidbody2D.isKinematic = false;

            // start countdown towards this piece fading out
            BreakablePiece piece = child.GetComponent<BreakablePiece>();
            piece.CountDown();

            // apply explosions!
            switch (explosion)
            {
                case Type.Explosion:
                    rigidbody2D.AddExplosionForce(2000, transform.position, 20);
                break;

                case Type.Directional_Explosion:
                    int force = (hitFrom == RIGHT) ? -50 : 50;
                    rigidbody2D.AddForce(new Vector3(force, 50, 50), ForceMode2D.Impulse);
                    // params for ShakeCamera = duration, strength, vibrato, randomness
                    Messenger.Broadcast<float, float, int, float>("shake camera", .7f, .4f, 20, 3f);
                break;

                case Type.Slump:
                    rigidbody2D.AddExplosionForce(250, transform.position, 3);
                break;

                case Type.Directional_Slump:
                    int direction = (hitFrom == RIGHT) ? 1 : -1;
                    rigidbody2D.AddForce(new Vector3(0, -100, 0));
                    rigidbody2D.AddExplosionForce(800,
                        new Vector3(transform.position.x + direction, transform.position.y + .5f, transform.position.z), 2);
                break;

                case Type.Geyser:
                    rigidbody2D.AddForce(new Vector3(0, -75, 0), ForceMode2D.Impulse);
                break;

                default:
                    rigidbody2D.AddExplosionForce(2000, transform.position, 5);
                break;
            }
        }
    }
}





