using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        _moveVector = (Vector3.zero - transform.position).normalized;
    }
}
