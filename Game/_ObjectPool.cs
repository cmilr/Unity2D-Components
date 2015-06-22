using UnityEngine;
using System.Collections;

public class _ObjectPool : BaseBehaviour {

    public _ObjectPool objectPool;
    public GameObject playerProjectile;
    public GameObject enemyProjectile;

    void Awake()
    {
        MakePseudoSingleton();
        playerProjectile = (GameObject)Resources.Load("Prefabs/Projectiles/PlayerProjectile", typeof(GameObject));
        enemyProjectile = (GameObject)Resources.Load("Prefabs/Projectiles/EnemyProjectile", typeof(GameObject));
        playerProjectile.CreatePool(2);
        enemyProjectile.CreatePool(2);
    }

    void MakePseudoSingleton()
    {
        if (objectPool == null)
        {
            DontDestroyOnLoad(gameObject);
            objectPool = this;
        }
        else if (objectPool != this)
        {
            Destroy(gameObject);
        }
    }
}




