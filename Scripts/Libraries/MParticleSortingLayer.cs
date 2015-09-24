using UnityEngine;
using System.Collections;

public class MParticleSortingLayer : CacheBehaviour {

    public string setSortingLayer;
    public int setSortingOrder = 0;

    void Start ()
    {
        // Set the sorting layer of the particle system.
        GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerName = setSortingLayer;
        GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingOrder = setSortingOrder;
    }
}

