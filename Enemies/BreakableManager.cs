using Matcha.Unity;
using UnityEngine;
using UnityEngine.Assertions;

public class BreakableManager : BaseBehaviour
{
	private enum Type { Explosion, Directional_Explosion, Slump, Directional_Slump, Geyser };
	private Type disintegration;
	private Sprite[] slices;
	private new Transform transform;

	void Awake()
	{
		transform = GetComponent<Transform>();
		Assert.IsNotNull(transform);
	}

	void Start()
	{
		InstantiateBreakablePieces();
	}

	void InstantiateBreakablePieces()
	{
		//find and load the sprite for this particular creature
		slices = Resources.LoadAll<Sprite>("Sprites/BreakableCreatures/" + transform.parent.name + "_BREAK");
		Assert.IsNotNull(slices);

		//find and load the 'BreakablePiece' prefab
		var prefab = (GameObject)Resources.Load("Prefabs/Misc/BreakablePiece", typeof(GameObject));
		Assert.IsNotNull(prefab);

		//instantiate the prefab as many times as required via this loop
		for (int i = 0; i < slices.Length; i += 2)
		{
			var newPiece = Instantiate(prefab, transform.position, Quaternion.identity) as GameObject;
			Assert.IsNotNull(newPiece);
			newPiece.transform.parent = gameObject.transform;
			newPiece.GetComponent<BreakablePiece>().Init(i, slices[i]);
		}

		//deactivate after a delay, so other components have time to cache a reference
		Invoke("MakeInactive", .5f);
	}

	public void MakeActive()
	{
		gameObject.SetActive(true);
	}

	void MakeInactive()
	{
		gameObject.SetActive(false);
	}

	void ExplodeCreature(Hit hit)
	{
		switch (hit.weapon.style)
		{
			case Weapon.Style.Melee:
				DirectionalSlump(hit);
				break;
			case Weapon.Style.Ranged:
				Explode(hit);
				break;
		}
	}

	void DirectionalSlump(Hit hit)
	{
		gameObject.SetActive(true);

		//randomly choose an disintegration type
		switch (Rand.Range(1, 2))
		{
			case 0:
				disintegration = Type.Slump;
				break;
			case 1:
			case 2:
				disintegration = Type.Directional_Slump;
				break;
			default:
				Assert.IsTrue(false, "** Default Case Reached **");
				break;
		}

		//cycle through pieces and send them flying
		foreach (Transform child in transform)
		{
			int direction;

			//activate physics on this piece
			Rigidbody2D childRigidbody2D = child.GetComponent<Rigidbody2D>();
			childRigidbody2D.isKinematic = false;

			//start countdown towards this piece fading out
			BreakablePiece piece = child.GetComponent<BreakablePiece>();
			piece.CountDown();

			//apply disintegrations!
			switch (disintegration)
			{
				case Type.Slump:
					childRigidbody2D.AddExplosionForce(250, transform.position, 3);
					break;
				case Type.Directional_Slump:
					direction = (hit.horizontalSide == Side.Right) ? 1 : -1;
					childRigidbody2D.AddForce(new Vector3(0, -100, 0));
					childRigidbody2D.AddExplosionForce(2000, new Vector3(
						transform.position.x + direction,
						transform.position.y + .5f,
						transform.position.z), 2
					);
					break;
				default:
					Assert.IsTrue(false, "** Default Case Reached **");
					break;
			}
		}
	}

	void Explode(Hit hit)
	{
		gameObject.SetActive(true);

		//randomly choose an disintegration type
		switch (Rand.Range(1, 1))
		{
			case 0:
				disintegration = Type.Explosion;
				break;
			case 1:
				disintegration = Type.Directional_Explosion;
				break;
			case 2:
				disintegration = Type.Slump;
				break;
			case 3:
				disintegration = Type.Directional_Slump;
				break;
			case 4:
				disintegration = Type.Geyser;
				break;
			default:
				Assert.IsTrue(false, "** Default Case Reached **");
				break;
		}

		//cycle through pieces and send them flying
		foreach (Transform child in transform)
		{
			int direction;

			//activate physics on this piece
			Rigidbody2D childRigidbody2D = child.GetComponent<Rigidbody2D>();
			childRigidbody2D.isKinematic = false;

			//start countdown towards this piece fading out
			BreakablePiece piece = child.GetComponent<BreakablePiece>();
			piece.CountDown();

			//apply disintegrations!
			switch (disintegration)
			{
				case Type.Explosion:
					childRigidbody2D.AddExplosionForce(2000, transform.position, 20);
					break;
				case Type.Directional_Explosion:
					int force = (hit.horizontalSide == Side.Right) ? -50 : 50;
					childRigidbody2D.AddForce(new Vector3(force, 50, 50), ForceMode2D.Impulse);
					// params = duration, strength, vibrato, randomness.
					EventKit.Broadcast("shake camera", .4f, .1f, 10, 3f);
					break;
				case Type.Slump:
					childRigidbody2D.AddExplosionForce(250, transform.position, 3);
					break;
				case Type.Directional_Slump:
					direction = (hit.horizontalSide == Side.Right) ? 1 : -1;
					childRigidbody2D.AddForce(new Vector3(0, -100, 0));
					childRigidbody2D.AddExplosionForce(800, new Vector3(
						transform.position.x + direction,
						transform.position.y + .5f,
						transform.position.z), 2
					);
					break;
				case Type.Geyser:
					childRigidbody2D.AddForce(new Vector3(0, -75, 0), ForceMode2D.Impulse);
					break;
				default:
					Assert.IsTrue(false, "** Default Case Reached **");
					break;
			}
		}
	}
}
