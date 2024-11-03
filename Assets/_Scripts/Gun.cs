using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Gun : MonoBehaviour
{
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject bullet;

    private float _bulletsPerSecond = 12f;
    private float _fireCooldown = 0f;
    private bool _canFire = true;
    
    // Start is called before the first frame update
    void Start()
    {
        _canFire = true;
        _fireCooldown = 1f / _bulletsPerSecond;
    }

    // Update is called once per frame
    void Update()
    {
        LookAtMouse();
        
        return;

        if (!GameManager.Instance.IsAutoOn)
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
        GameObject bulletGo = Instantiate(bullet, bulletSpawnPoint.transform.position, Quaternion.identity);
        bulletGo.transform.up = transform.up;
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
    
}
