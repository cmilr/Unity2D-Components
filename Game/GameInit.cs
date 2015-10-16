using UnityEngine;
using System.Collections;
using DG.Tweening;
using Matcha.Lib;


public class GameInit : BaseBehaviour
{
    void Awake()
    {
        // initialize DOTween before first use.
        DOTween.Init(true, true, LogBehaviour.Verbose).SetCapacity(4000, 4000);

        // seed Random with current seconds;
        Random.seed = (int)System.DateTime.Now.Ticks;

        // DOTween.SetTweensCapacity(2000, 300);

        M.IgnoreLayerCollision2D("BodyCollider", "Platform", true);
        M.IgnoreLayerCollision2D("BodyCollider", "One-Way Platform", true);
        M.IgnoreLayerCollision2D("WeaponCollider", "Platform", true);
        M.IgnoreLayerCollision2D("WeaponCollider", "One-Way Platform", true);


        // for (int i = 0; i < 20; i++)
        //     Debug.Log(M.NextGaussian(25f, 5f, 1f, 50f));
    }
}
