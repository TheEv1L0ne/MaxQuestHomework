using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Fish : NetworkBehaviour
{
    [SerializeField] protected float speed = 4f;
    [SerializeField] protected int score = 10;
    [SerializeField] protected Color fishColor = Color.white;
    [SerializeField] protected string fishType = "";
    [SerializeField] protected int hp = 5;
    
    protected Vector3 MoveVector = Vector3.zero;
    protected bool FirstTimeEntered = false;
    
    private float _screenHeight;
    private float _screenWidth;

    public static Action<FishStats> FishKilledEvent = null;
    
    public override void OnNetworkSpawn()
    {
        _screenHeight = Camera.main.orthographicSize;
        _screenWidth = 16f / 9f * _screenHeight;
        InitStartDirection();
    }
    
    // Update is called once per frame
    private void Update()
    {
        if(!IsServer)
            return;
        
        MoveFish();
        EnteredScreenFirstTime();
        DestroyIfRunOut();
        ChangeFishDirection();
    }
    
    private void MoveFish()
    {
        transform.position += MoveVector * Time.deltaTime * speed;
    }

    protected abstract void ChangeFishDirection();
    
    protected virtual void RandomizeDirection()
    {
        var x = Random.Range(0, 2);
        MoveVector = x == 0 
            ? new Vector3(MoveVector.y, -MoveVector.x, 0f) 
            : new Vector3(-MoveVector.y, MoveVector.x, 0f);
    }
    
    private void InitStartDirection()
    {
        var pos = this.transform.position;
        
        var xMax = _screenWidth + 0.5f;
        var xMin = -xMax;

        var yMax = _screenHeight + 0.5f;
        var yMin = -yMax;

        if (pos.y > _screenHeight)
        {
            yMax = 0f;
        }

        if (pos.y < -_screenHeight)
        {
            yMin = 0f;
        }
        
        if(pos.x > _screenWidth)
        {
            xMax = 0f;
        }
        
        if(pos.x < -_screenWidth)
        {
            xMin = 0f;
        }
        
        var x = Random.Range(xMin, xMax);
        var y = Random.Range(yMin, yMax);

        var directionPoint = new Vector3(x, y, 0f);
        
        MoveVector = (directionPoint - transform.position).normalized;
    }
    
    private void DestroyIfRunOut()
    {
        if(!FirstTimeEntered) return;
        if (IsInside()) return;
        if (!IsServer) return;
        
        var networkObject = GetComponent<NetworkObject>();
        networkObject.Despawn();
    }

    private void EnteredScreenFirstTime()
    {
        if (FirstTimeEntered) return;
        if (!IsInside()) return;
        
        FirstTimeEntered = true;
    }

    private bool IsInside()
    {
        var pos = this.transform.position;
        return pos.x < _screenWidth & pos.x > -_screenWidth
               && pos.y < _screenHeight && pos.y > -_screenHeight;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!IsSpawned) return;
        if(!IsOwner) return;

        if (!other.CompareTag("Bullet")) return;
        
        var bullet = other.GetComponent<PlayerBullet>();
        DestroyedBulletServerRpc(bullet.bulletId, bullet.OwnerId);
            
        hp -= 1;
        if (hp > 0) return;
        
        SendKilledServerRpc(bullet.OwnerId);
        DestroyObjectServerRpc();
    }
    
    [ServerRpc]
    private void SendKilledServerRpc(ulong ownerId)
    {;
        KilledFishClientRpc(ownerId);
    }

    [ClientRpc]
    private void KilledFishClientRpc(ulong ownerId)
    {
        FishKilledEvent?.Invoke(new FishStats
        {
            FishColor = fishColor,
            Score = score,
            FishType = fishType,
            KillerId = ownerId
        });
    }

    [ServerRpc]
    private void DestroyedBulletServerRpc(string bulletId, ulong ownerId)
    {
        if(!IsSpawned) return;
        
        DestroyedBulletClientRpc(bulletId, ownerId);
    }
    
    [ClientRpc]
    private void DestroyedBulletClientRpc(string bulletId, ulong ownerId)
    {
        BulletSpawner.Instance.ReturnBulletToPool(bulletId, ownerId);
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void DestroyObjectServerRpc()
    {
        if(!IsSpawned) return;
        
        NetworkObject.Despawn(true);
    }
    
    public struct FishStats
    {
        public Color FishColor;
        public int Score;
        public string FishType;
        public ulong KillerId;
    }
}
