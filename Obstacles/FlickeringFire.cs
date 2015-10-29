using UnityEngine;
using System.Collections;
using Matcha.Unity;
using Matcha.Dreadful;

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
            yield return new WaitForSeconds(Rand.Range(.01f, .3f));
        }
    }
}
