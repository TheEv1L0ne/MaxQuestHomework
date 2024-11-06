using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BulletSpawner : MonoBehaviour
{
    public static BulletSpawner Instance = null;
    
    [SerializeField] private Transform bulletRoot;
    [SerializeField] private PlayerBullet playerBullet;

    private const int PoolAmount = 50;

    private List<PlayerBullet> _bulletPool;
    
    private void Awake()
    {
        if(Instance != null)
            Destroy(this.gameObject);

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        _bulletPool = new List<PlayerBullet>();
        for (var i = 0; i < PoolAmount; i++)
        {
            var bullet = InstantiateBullet();
            _bulletPool.Add(bullet);
        }
    }

    private PlayerBullet InstantiateBullet()
    {
        var bullet = Instantiate(this.playerBullet, Vector3.zero, Quaternion.identity);
        bullet.gameObject.SetActive(false);
        bullet.transform.parent = bulletRoot;
        return bullet;
    }

    public PlayerBullet GetBulletFromPool()
    {
        PlayerBullet bullet = null;
        
        for (var i = 0; i < PoolAmount; i++)
        {
            if (!_bulletPool[i].gameObject.activeInHierarchy)
            {
                bullet =  _bulletPool[i];
            }
        }

        if (bullet == null)
        {
            bullet = InstantiateBullet();
            _bulletPool.Add(bullet);
        }
        
        bullet.gameObject.SetActive(true);

        return bullet;
    }

    public void ReturnBulletToPool(PlayerBullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }
}
