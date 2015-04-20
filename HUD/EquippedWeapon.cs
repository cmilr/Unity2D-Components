using UnityEngine;
using System.Collections;

public class EquippedWeapon : CacheBehaviour {

	private Camera mainCamera;

	void Start ()
    {
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        transform.position = mainCamera.ScreenToWorldPoint(new Vector3 (
            Screen.width / 2,
            Screen.height - HUD_TOP_MARGIN,
            HUD_Z));
	}
}
