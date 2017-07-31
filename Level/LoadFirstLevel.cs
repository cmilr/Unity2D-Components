using UnityEngine.SceneManagement;
using UnityEngine;

// allows Unity to pre-load assets for first actual level.
public class LoadFirstLevel : BaseBehaviour {

	void Start() 
	{
		Debug.Log("LoadFirstLevel called");
		SceneManager.LoadScene("Level1");
	}
}
