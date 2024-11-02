using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private float speed = 4f;
    
    private Vector3 _moveVector = Vector3.zero;
    
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
        float ortoSize = Camera.main.orthographicSize;
        
        float width = 16f / 9f * ortoSize;

        var pos = this.transform.position;

        if (pos.y > ortoSize ||  pos.y < -ortoSize)
        {
            var moveV = _moveVector;
            moveV.y = -moveV.y;
            _moveVector = moveV;
        }
        
        if(pos.x < -width || pos.x > width)
        {
            var moveV = _moveVector;
            moveV.x = -moveV.x;
            _moveVector = moveV;
        }
    }
}
