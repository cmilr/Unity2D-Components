using UnityEngine;
using System.Collections;


public class SetSortingOrder : MonoBehaviour
{
    public string setSortingLayerTo = "Default";
    public int setOrderTo;

    void Start ()
    {
        renderer.sortingLayerName = setSortingLayerTo;
        renderer.sortingOrder = setOrderTo;
    }
}
