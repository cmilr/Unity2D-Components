using UnityEngine;
using System.Collections;


public class SetSortingOrder : BaseBehaviour
{
    public string setSortingLayerTo = "Default";
    public int setOrderTo;

    void Start ()
    {
        renderer.sortingLayerName = setSortingLayerTo;
        renderer.sortingOrder = setOrderTo;
    }
}
