using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Fish : MonoBehaviour
{
    [SerializeField] protected float speed = 4f;
    [SerializeField] protected int score = 10;
    [SerializeField] protected Color fishColor = Color.white;
    [SerializeField] protected string fishType = "";
    
    private Vector3 _moveVector = Vector3.zero;

    private bool _firstTimeEnetered = false;
    
    // Start is called before the first frame update
    void Start()
    {
        InitStartDirection();
    }

    // Update is called once per frame
    void Update()
    {
        MoveFish();
        FirstTimeEntered();
        DestroyIfRunOut();
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
    
    private void DestroyIfRunOut()
    {
        if(!_firstTimeEnetered)
            return;

        if (IsInside())
            return;
        
        GameManager.Instance.AddScore(Color.white, 0, "");
        Destroy(this.gameObject);
    }

    private void FirstTimeEntered()
    {
        if (_firstTimeEnetered)
            return;
        
        if (IsInside())
        {
            _firstTimeEnetered = true;
        }
    }

    private bool IsInside()
    {
        var pos = this.transform.position;
        return pos.x < GameManager.Instance.Width & pos.x > -GameManager.Instance.Width
               && pos.y < GameManager.Instance.Height && pos.y > -GameManager.Instance.Height;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            GameManager.Instance.AddScore(fishColor, score, fishType);
            Destroy(this.gameObject);
        }
    }
}
