using UnityEngine;
using System.Collections;


// PERSISTENT SINGLETON
// ~~~~~~~~~~~~~~~~~~~~
public class GameDataSingleton : BaseBehaviour
{
	private static GameDataSingleton _instance;
	public static GameDataSingleton instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = GameObject.FindObjectOfType<GameDataSingleton>();

				// tell unity not to destroy this object when loading a new scene.
				DontDestroyOnLoad(_instance.gameObject);
			}

			return _instance;
		}
	}

	void Awake()
	{
		if (_instance == null)
		{
			// if I am the first instance, make me the Singleton.
			_instance = this;
			DontDestroyOnLoad(this);
		}
		else
		{
			// if a Singleton already exists and you find
			// another reference in scene, destroy it.
			if (this != _instance)
			{ Destroy(this.gameObject); }
		}
	}
}
