using UnityEngine;


public class TileEntity : MonoBehaviour
{
	void OnBecameVisible()
	{
		gameObject.SetActive(true);
	}

	void OnBecameInvisible()
	{
		gameObject.SetActive(false);
	}
}
