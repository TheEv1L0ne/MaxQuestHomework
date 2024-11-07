using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private float speed = 12f;

    public ulong OwnerId;
    public string bulletId;
    
    private Vector3 _moveVector = Vector3.zero;

    private int _lastBounceY = 0;
    private int _lastBounceX = 0;
    
    private float _screenHeight;
    private float _screenWidth;

    private SpriteRenderer _spriteRenderer;
    
    public void Init(Data data)
    {
        OwnerId = data.OwnerId;
        bulletId = data.Id;
        transform.position = data.StartPos;
        transform.up = data.Direction;
        _moveVector = transform.up.normalized;
        _screenHeight = Camera.main.orthographicSize;
        _screenWidth = 16f / 9f * _screenHeight;
        
        _lastBounceY = 0;
        _lastBounceX = 0;

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.color = data.playerColor;
    }
    
    void Update()
    {
        MoveBullet();
        CheckForBounce();
    }

    private void MoveBullet()
    {
        transform.position += _moveVector * (Time.deltaTime * speed);
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
        if (!other.CompareTag("Fish")) return;
        
        // BulletSpawner.Instance.ReturnBulletToPool(bulletId, OwnerId);
    }
    
    public struct Data
    {
        public ulong OwnerId;
        public string Id;
        public Vector3 StartPos;
        public Vector3 Direction;
        public Color playerColor;
    }
}
