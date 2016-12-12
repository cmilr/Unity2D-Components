using DG.Tweening;
using Matcha.Unity;
using UnityEngine;

public class GameInit : BaseBehaviour
{
	void Start()
	{
		// set profiler frames to max.
		Profiler.maxNumberOfSamplesPerFrame = 8000000;

		// initialize DOTween before first use.
		DOTween.Init(true, true, LogBehaviour.Verbose).SetCapacity(2000, 2000);
		DOTween.showUnityEditorReport = false;
		DOTween.defaultAutoKill = false;

		// seed Rand with current seconds;
		Rand.Seed();
		
		// run explicit garbage collection after all caching has been performed
		// in the Start() and Awake() scripts. this runs on level reload as well.
		Invoke("PostInitGC", .5f);
	}
	
	void PostInitGC()
	{
		System.GC.Collect();
	}
}
