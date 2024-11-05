using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerShoot : NetworkBehaviour
{
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject bullet;

    private const int BulletsPerSecond = 12;
    private bool CanFire => (_lastFired + 1f/BulletsPerSecond) <= Time.time;

    private bool _isAutoOn = false;

    private float _lastFired = float.MinValue;
    
    public override void OnNetworkSpawn()
    {
        
        if(!IsOwner)
            Destroy(this);
        
        AutofireController.ToggleValueChangedEvent += OnToggle;
    }
    
    public override void OnNetworkDespawn()
    {
        AutofireController.ToggleValueChangedEvent -= OnToggle;
    }
    
    private void OnToggle(bool obj)
    {
        _isAutoOn = obj;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isAutoOn)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }
            
                FireBullet();
            } 
        }
        else
        {
            FireBullet();
        }
    }

    private void FireBullet()
    {
        if (!CanFire) return;
        
        _lastFired = Time.time;
        SpawnBullet();
    }
    
    private void SpawnBullet()
    {
        SpawnBulletServerRpc();
    }
    
    [ServerRpc]
    private void SpawnBulletServerRpc()
    {
        GameObject bulletGo = Instantiate(bullet, bulletSpawnPoint.transform.position, Quaternion.identity);
        bulletGo.transform.up = transform.up;
        bulletGo.GetComponent<Bullet>().someID = OwnerClientId.ToString();
        NetworkObject networkObject = bulletGo.GetComponent<NetworkObject>();
        networkObject.Spawn(true);
    }
}
