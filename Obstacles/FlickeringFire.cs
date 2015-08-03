using UnityEngine;
using System.Collections;
using Matcha.Dreadful.FX;

public class FlickeringFire : CacheBehaviour {

    public float minIntensity;
    public float maxIntensity;

	void Start()
    {
        StartCoroutine(FlickerFXLoop());
	}

    IEnumerator FlickerFXLoop() {

        while (true)
        {
            MFX.Flicker(light, minIntensity, maxIntensity);
            yield return new WaitForSeconds(UnityEngine.Random.Range(.01f, .3f));
        }
    }
}
