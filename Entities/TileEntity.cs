using UnityEngine;


public class TileEntity : BaseBehaviour
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
