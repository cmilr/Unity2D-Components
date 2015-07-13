using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectilePool : BaseBehaviour {

    public static ProjectilePool current;
    public GameObject pooledObject;
    public int pooledAmount = 20;
    public bool increaseWhenNeeded = true;
    List<GameObject> pooledObjects;

    void Awake ()
    {
        current = this;
    }

    void Start()
    {
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < pooledAmount; i++)
        {
            GameObject obj = (GameObject) Instantiate(pooledObject);
            obj.SetActive(false);
            // allocate memory for references at instantiation
            obj.GetComponent<ProjectileContainer>().AllocateMemory();
            pooledObjects.Add(obj);
        }
    }

    public GameObject Spawn()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        if (increaseWhenNeeded)
        {
            GameObject obj = (GameObject) Instantiate(pooledObject);
            obj.SetActive(false);
            // allocate memory for references at instantiation
            obj.GetComponent<ProjectileContainer>().AllocateMemory();
            pooledObjects.Add(obj);
            return obj;
        }

        return null;
    }
}




