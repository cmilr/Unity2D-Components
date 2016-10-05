using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ProjectilePool : BaseBehaviour
{
	public static ProjectilePool current;
	public GameObject pooledObject;
	public int pooledAmount = 20;
	public bool increaseWhenNeeded = true;
	private List<GameObject> pooledObjects;
	
	void Awake()
	{
		current = this;
	}

	void Start()
	{
		pooledObjects = new List<GameObject>();
		
		for (int i = 0; i < pooledAmount; i++)
		{
			var obj = Instantiate(pooledObject);
			Assert.IsNotNull(obj);
			
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
			var obj = Instantiate(pooledObject);
			Assert.IsNotNull(obj);
			
			obj.SetActive(false);

			// allocate memory for references at instantiation
			obj.GetComponent<ProjectileContainer>().AllocateMemory();
			pooledObjects.Add(obj);

			return obj;
		}

		return null;
	}
}
