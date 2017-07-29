using DG.Tweening;
using Matcha.Unity;

public class GameInit : BaseBehaviour
{
	void Start()
	{
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
