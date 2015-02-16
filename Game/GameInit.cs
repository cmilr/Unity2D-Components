using UnityEngine;
using System.Collections;
using DG.Tweening;
using Matcha.Lib;


public class GameInit : BaseBehaviour
{
	void Awake()
	{
		// seed Random with current seconds;
		Random.seed = (int)System.DateTime.Now.Ticks;

		// initialize DOTween before first use.
		DOTween.Init(true, true, LogBehaviour.Verbose).SetCapacity(200, 10);
		DOTween.SetTweensCapacity(2000, 100);
	}
}
