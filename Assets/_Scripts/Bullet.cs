using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    [SerializeField] private float speed = 4f;
    
    private Vector3 _moveVector = Vector3.zero;

    private int _lastBounceY = 0;
    private int _lastBounceX = 0;

    public Gun parent;
    public string someID;

    private float _screenHeight;
    private float _screenWidth;

    // Start is called before the first frame update
    public override void OnNetworkSpawn()
    {
        _moveVector = transform.up.normalized;
        _screenHeight = Camera.main.orthographicSize;
        _screenWidth = 16f / 9f * _screenHeight;
    }

    // Update is called once per frame
    void Update()
    {
        MoveBullet();
        CheckForBounce();
    }

    private void MoveBullet()
    {
        transform.position += _moveVector * Time.deltaTime * speed;
    }

    private void CheckForBounce()
    {
        var pos = this.transform.position;

        if (pos.y > _screenHeight && _lastBounceY != 1)
        {
            _lastBounceY = 1;
            ChangeY();
        }

        if (pos.y < -_screenHeight && _lastBounceY != -1)
        {
            _lastBounceY = -1;
            ChangeY();
        }
        
        if(pos.x > _screenWidth && _lastBounceX != 1)
        {
            _lastBounceX = 1;
            ChangeX();
        }
        
        if(pos.x < -_screenWidth && _lastBounceX != -1)
        {
            _lastBounceX = -1;
            ChangeX();
        }
    }

    private void ChangeY()
    {
        var moveV = _moveVector;
        moveV.y = -moveV.y;
        _moveVector = moveV;
    }

    private void ChangeX()
    {
        var moveV = _moveVector;
        moveV.x = -moveV.x;
        _moveVector = moveV;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!IsOwner) return;
        
        if(!IsSpawned) return;
        
        if (other.CompareTag("Fish"))
        {
            // parent.DestroyServerRpc(this.gameObject);
            DestroyServerRpc();
        }
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void DestroyServerRpc()
    {
        GetComponent<NetworkObject>().Despawn();
    }
}
