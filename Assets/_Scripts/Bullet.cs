using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private float speed = 4f;
    
    private Vector3 _moveVector = Vector3.zero;

    private int _lastBounceY = 0;
    private int _lastBounceX = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        _moveVector = transform.up.normalized;
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

        if (pos.y > GameManager.Instance.Height && _lastBounceY != 1)
        {
            _lastBounceY = 1;
            ChangeY();
        }

        if (pos.y < -GameManager.Instance.Height && _lastBounceY != -1)
        {
            _lastBounceY = -1;
            ChangeY();
        }
        
        if(pos.x > GameManager.Instance.Width && _lastBounceX != 1)
        {
            _lastBounceX = 1;
            ChangeX();
        }
        
        if(pos.x < -GameManager.Instance.Width && _lastBounceX != -1)
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
        if (other.CompareTag("Fish"))
        {
            Destroy(this.gameObject);
        }
    }
}
