using UnityEngine;
using System.Collections;
using Matcha.Dreadful.FX;

public class FlickeringFire : CacheBehaviour {

	void Start()
    {
        StartCoroutine(FlickerFXLoop());
	}

    IEnumerator FlickerFXLoop() {

        MFX.Flicker(light, .1f, 4.5f);

        yield return new WaitForSeconds(UnityEngine.Random.Range(.01f, .3f));

        StartCoroutine(FlickerFXLoop());
    }

}
