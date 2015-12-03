using DG.Tweening;
using Matcha.Unity;
using System.Collections;
using UnityEngine;

public class GameInit : BaseBehaviour
{
	void Awake()
	{
		// initialize DOTween before first use.
		DOTween.Init(true, true, LogBehaviour.Verbose).SetCapacity(300, 300);
		DOTween.showUnityEditorReport = true;

		// seed Rand with current seconds;
		Rand.Seed();
	}
}
