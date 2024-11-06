using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class Fish : NetworkBehaviour
{
    [SerializeField] protected float speed = 4f;
    [SerializeField] protected int score = 10;
    [SerializeField] protected Color fishColor = Color.white;
    [SerializeField] protected string fishType = "";
    [SerializeField] protected int hp = 5;
    
    private Vector3 _moveVector = Vector3.zero;

    protected bool firstTimeEnetered = false;

    private float _changeFrequency = 2f;
    private float _timeLeftBeforeChange = 0f;
    
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
    void Update()
    {
        if(!IsServer)
            return;
        
        MoveFish();
        FirstTimeEntered();
        DestroyIfRunOut();
        ChangeFishDirection();
    }
    
    private void MoveFish()
    {
        transform.position += _moveVector * Time.deltaTime * speed;
    }

    private void ChangeFishDirection()
    {
        if(!firstTimeEnetered)
            return;

        if (_timeLeftBeforeChange <= 0f)
        {
            _timeLeftBeforeChange = _changeFrequency;
            RandomizeDirection();
        }
        else
        {
            _timeLeftBeforeChange -= Time.deltaTime;
        }
    }

    private void RandomizeDirection()
    {
        int x = Random.Range(0, 2);
        _moveVector = x == 0 
            ? new Vector3(_moveVector.y, -_moveVector.x, 0f) 
            : new Vector3(-_moveVector.y, _moveVector.x, 0f);
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
        
        _moveVector = (directionPoint - transform.position).normalized;
    }
    
    private void DestroyIfRunOut()
    {
        if(!firstTimeEnetered)
            return;

        if (IsInside())
            return;
        
        //GameManager.Instance.AddScore(Color.white, 0, "");

        if (IsServer)
        {
            NetworkObject networkObject = GetComponent<NetworkObject>();
            networkObject.Despawn();
        }
    }

    private void FirstTimeEntered()
    {
        if (firstTimeEnetered)
            return;
        
        if (IsInside())
        {
            _timeLeftBeforeChange = _changeFrequency;
            firstTimeEnetered = true;
        }
    }

    private bool IsInside()
    {
        var pos = this.transform.position;
        return pos.x < _screenWidth & pos.x > -_screenWidth
               && pos.y < _screenHeight && pos.y > -_screenHeight;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!IsOwner) return;
        
        if (other.CompareTag("Bullet"))
        {
            PlayerBullet bullet = other.GetComponent<PlayerBullet>();
            var ownerId = bullet.OwnerId;
            
            hp -= 1;
            if (hp <= 0)
            {
                SendKilledServerRpc(ownerId);
                DestroyObjectServerRpc();
            }
        }
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
            fishColor = fishColor,
            score = score,
            fishType = fishType,
            killerId = ownerId
        });
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void DestroyObjectServerRpc()
    {
        if(!IsSpawned) return;
        
        NetworkObject.Despawn(true);
    }
    
    public struct FishStats
    {
        public Color fishColor;
        public int score;
        public string fishType;
        public ulong killerId;
    }
}
