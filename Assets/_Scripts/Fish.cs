using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Fish : MonoBehaviour
{
    [SerializeField] private float speed = 4f;
    private Vector3 _moveVector = Vector3.zero;
    
    // Start is called before the first frame update
    void Start()
    {
        InitStartDirection();
    }

    // Update is called once per frame
    void Update()
    {
        MoveFish();
    }
    
    private void MoveFish()
    {
        transform.position += _moveVector * Time.deltaTime * speed;
    }

    private void InitStartDirection()
    {
        var pos = this.transform.position;
        
        var xMax = GameManager.Instance.Width + 0.5f;
        var xMin = -xMax;

        var yMax = GameManager.Instance.Height + 0.5f;
        var yMin = -yMax;

        if (pos.y > GameManager.Instance.Height)
        {
            yMax = 0f;
        }

        if (pos.y < -GameManager.Instance.Height)
        {
            yMin = 0f;
        }
        
        if(pos.x > GameManager.Instance.Width)
        {
            xMax = 0f;
        }
        
        if(pos.x < -GameManager.Instance.Width)
        {
            xMin = 0f;
        }
        
        var x = Random.Range(xMin, xMax);
        var y = Random.Range(yMin, yMax);

        var directionPoint = new Vector3(x, y, 0f);
        
        _moveVector = (directionPoint - transform.position).normalized;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            Destroy(this.gameObject);
        }
    }
}
