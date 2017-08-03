using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ObjectPool : BaseBehaviour
{
	public static ObjectPool projectile;
	public GameObject pooledObject;
	public int pooledAmount = 20;
	public bool willGrow = true;
	private List<GameObject> pooledObjects;

	void Awake()
	{
		projectile = this;
	}

	void Start()
	{
		pooledObjects = new List<GameObject>();
		
		for (int i = 0; i < pooledAmount; i++)
		{
			var obj = Instantiate(pooledObject);
			Assert.IsNotNull(obj);
			
			obj.SetActive(false);

			// allocate memory for references at instantiation.
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

		if (willGrow)
		{
			var obj = Instantiate(pooledObject);
			Assert.IsNotNull(obj);
			
			obj.SetActive(false);

			// allocate memory for references at instantiation.
			obj.GetComponent<ProjectileContainer>().AllocateMemory();
			pooledObjects.Add(obj);

			return obj;
		}

		return null;
	}
}
