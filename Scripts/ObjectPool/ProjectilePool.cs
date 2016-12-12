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
			InstantiateNewGameObject();
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
			return InstantiateNewGameObject();
		}

		return null;
	}

	GameObject InstantiateNewGameObject()
	{
		var obj = Instantiate(pooledObject);
		Assert.IsNotNull(obj);

		obj.GetComponent<ProjectileContainer>().CacheRefsThenDisable();
		obj.GetComponent<Rigidbody2D>().isKinematic = true;

		// allocate memory for references at instantiation
		obj.GetComponent<ProjectileContainer>().AllocateMemory();
		pooledObjects.Add(obj);

		return obj;
	}
}
