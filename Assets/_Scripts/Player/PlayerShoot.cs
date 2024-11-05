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
    
    private float _fireCooldown = 0f;
    private bool _canFire = true;

    private bool _isAutoOn = false;
    
    public override void OnNetworkSpawn()
    {
        if(!IsOwner)
            Destroy(this);
        
        _canFire = true;
        _fireCooldown = 1f / BulletsPerSecond;
        
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
            
                if (_canFire)
                {
                    _canFire = false;
                    SpawnBullet();
                }
            } 
        }
        else
        {
            if (_canFire)
            {
                _canFire = false;
                SpawnBullet();
            } 
        }
        
        GunCooldown();
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
    
    private void GunCooldown()
    {
        if (_canFire) return;
        
        if (_fireCooldown > 0)
        {
            _fireCooldown -= Time.deltaTime;
        }
        else
        {
            _canFire = true;
            _fireCooldown = 1f / BulletsPerSecond;
        }
    }
}
