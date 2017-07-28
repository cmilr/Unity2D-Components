using UnityEngine;
using UnityEngine.Assertions;

[ExecuteInEditMode]
public class AutoEnableTouchControls : BaseBehaviour
{
	private GameObject canvasGo;
	private GameObject eventSystem;

	void Start()
	{
		canvasGo = gameObject.transform.Find("Canvas").gameObject;
		Assert.IsNotNull(canvasGo);

		eventSystem = gameObject.transform.Find("EventSystem").gameObject;
		Assert.IsNotNull(eventSystem);

		#if UNITY_STANDALONE
			canvasGo.SetActive(false);
			eventSystem.SetActive(false);
		#endif

		#if UNITY_IOS
			canvasGo.SetActive(true);
			eventSystem.SetActive(true);
		#endif
	}
}
