using UnityEngine;
using System.Collections;
using DG.Tweening;
using Matcha.Unity;


public class GameInit : BaseBehaviour
{
    void Awake()
    {
        // initialize DOTween before first use.
        DOTween.Init(true, true, LogBehaviour.Verbose).SetCapacity(4000, 4000);

        // seed Rand with current seconds;
        Rand.Seed();
    }
}
