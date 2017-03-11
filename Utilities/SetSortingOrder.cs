using System.Collections;
using UnityEngine;

public class SetSortingOrder : BaseBehaviour
{
	public string setSortingLayerTo = "Default";
	public int setOrderTo;

	void Start()
	{
		GetComponent<Renderer>().sortingLayerName = setSortingLayerTo;
		GetComponent<Renderer>().sortingOrder = setOrderTo;
	}
}
