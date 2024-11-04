using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class Gun : NetworkBehaviour
{
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject bullet;

    private float _bulletsPerSecond = 12f;
    private float _fireCooldown = 0f;
    private bool _canFire = true;

    private bool _isAutoOn = false;

    public static Action<Fish.FishStats> AddKilledFishEvent = null;

    public NetworkVariable<int> nekiInt = new NetworkVariable<int>();
    

    public string Id = "";
    
    public override void OnNetworkSpawn()
    {
        Id = NetworkObjectId.ToString();
        
        _canFire = true;
        _fireCooldown = 1f / _bulletsPerSecond;
        
        if(!IsOwner) return;

        var pos = new Vector3();
        
        if (OwnerClientId % 4 == 0)
        {
            transform.position = new Vector3(-3f, -5f, 0f);
        }
        if (OwnerClientId % 4 == 1)
        {
            transform.position = new Vector3(3f, -5f, 0f);
        }
        if (OwnerClientId % 4 == 2)
        {
            transform.position = new Vector3(-3f, 5f, 0f);
        }
        if (OwnerClientId % 4 == 3)
        {
            transform.position = new Vector3(3f, 5f, 0f);
        }
        
        

        AutofireController.ToggleValueChangedEvent += OnToggle;
        // Fish.FishKilledEvent += stats =>
        // {
        //     Debug.Log($"Got Id: {stats.clientId}");
        // };
    }

    public override void OnNetworkDespawn()
    {
        if(!IsOwner) return;
        AutofireController.ToggleValueChangedEvent -= OnToggle;
    }

    private void OnToggle(bool obj)
    {
        _isAutoOn = obj;
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsOwner || !Application.isFocused)
            return;
        
        LookAtMouse();
        
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

    private void LookAtMouse()
    {
        Vector2 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mouseScreenPosition - (Vector2) transform.position).normalized;
        transform.up = direction;
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
        bulletGo.GetComponent<Bullet>().parent = this;
        bulletGo.GetComponent<Bullet>().someID = OwnerClientId.ToString();
        NetworkObject networkObject = bulletGo.GetComponent<NetworkObject>();
        networkObject.Spawn(true);
    }
    
    private void GunCooldown()
    {
        if (!_canFire)
        {
            if (_fireCooldown > 0)
            {
                _fireCooldown -= Time.deltaTime;
            }
            else
            {
                _canFire = true;
                _fireCooldown = 1f / _bulletsPerSecond;
            }
        }
    }

    public void AddStats(Fish.FishStats stats)
    {
        Debug.Log($"Event by: {Id}");
        AddKilledFishEvent?.Invoke(stats);
    }
}
