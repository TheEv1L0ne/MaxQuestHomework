using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class PlayerShoot : NetworkBehaviour
{
    [SerializeField] private Transform bulletSpawnPoint;

    private const int BulletsPerSecond = 12;
    private bool CanFire => (_lastFired + 1f/BulletsPerSecond) <= Time.time;

    private bool _isAutoOn = false;

    private float _lastFired = float.MinValue;
    
    public override void OnNetworkSpawn()
    {
        if(!IsOwner)
            return;
        
        AutofireController.ToggleValueChangedEvent += OnToggle;
    }
    
    public override void OnNetworkDespawn()
    {
        if(!IsOwner)
            return;
        
        AutofireController.ToggleValueChangedEvent -= OnToggle;
    }
    
    private void OnToggle(bool obj)
    {
        _isAutoOn = obj;
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsOwner)
            return;
        
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
        // SpawnBulletServerRpc();
        
        var direction = transform.up;
        RequestFireServerRpc(direction, OwnerClientId);
        
        ExecuteSHoot(direction, OwnerClientId);
    }
    
    [ServerRpc]
    private void RequestFireServerRpc(Vector3 direction, ulong id)
    {
        FireClientRpc(direction,id);
    }

    [ClientRpc]
    private void FireClientRpc(Vector3 direction,ulong id)
    {
        if(!IsOwner)
            ExecuteSHoot(direction, id);
    }

    private void ExecuteSHoot(Vector3 direction, ulong id)
    {
        var bullet = BulletSpawner.Instance.GetBulletFromPool();
        bullet.Init(bulletSpawnPoint.position, direction, id);
    }
}
