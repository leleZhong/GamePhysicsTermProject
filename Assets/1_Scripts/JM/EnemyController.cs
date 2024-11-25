using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float _speed; // 이동 속도
    public Rigidbody2D _rd;
    public Transform _myTF;

    [Header("Health Settings")]
    public int _hp;

    [Header("Visual Settings")]
    public Sprite[] _images; // 적 상태를 나타내는 스프라이트 배열
    public SpriteRenderer _spriteRenderer;

    [Header("Attack Settings")]
    public Transform _bullet;
    public bool _isShooting;
    float _currentTime; // 발사 간격 타이머

    void Awake()
    {
        _rd = GetComponent<Rigidbody2D>();
        _myTF = GetComponent<Transform>();
    }

    void Start()
    {
        _currentTime = 0;
        _rd.velocity = _myTF.up * _speed;
    }

    void Update()
    {
        if(_bullet)
        {
            if(_currentTime < Time.time)
            {
                Fire();
                _currentTime = Time.time + Random.Range(2f, 4f);
            }
        }
    }

    public void OnHit(int dmg)
    {
        _hp -= dmg;
        _spriteRenderer.sprite = _images[1];
        Invoke("ReturnImage", 0.1f);
        if (_hp <= 0)
            Destroy(gameObject);
    }
    
    void ReturnImage()
    {
        _spriteRenderer.sprite = _images[0];
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "bullet" :
                Bullet bullet = other.GetComponent<Bullet>();

                OnHit(bullet._pow);
                PoolManager.Despawn(other.gameObject);
                break;
            
            case "Player" :
                other.gameObject.SetActive(false);
                break;
        }
    }

    void Fire()
    {
        // 외부클래스에서 가져오고 싶으면 싱글톤패턴 적용
        Vector3 dir = GameManager.Instance._playerTF.position - _myTF.position;
        // float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;

        Quaternion rotate = Quaternion.identity;
        // rotate.eulerAngles = new Vector3(0, 0, angle);
        rotate = Quaternion.FromToRotation(Vector3.up, dir);

        if (_bullet != null)
        {
            Instantiate(_bullet, _myTF.position, rotate);
        }
    }
}
