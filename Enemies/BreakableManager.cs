using UnityEngine;
using System.Collections;

public class BreakableManager : CacheBehaviour {

    private Sprite[] slices;


	void Start () {

        slices = Resources.LoadAll<Sprite>("Sprites/BreakableCreatures/" + transform.parent.name + "_BREAK");
        Debug.Log(slices[0]);
        Debug.Log(slices.Length);

	}
}
