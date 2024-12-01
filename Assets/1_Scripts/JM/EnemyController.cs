using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float _speed; // 이동 속도
    public Rigidbody2D _rd;
    public Transform _myTF;
    public Animator animator;


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
        animator.SetBool("isWalk", true);
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

    void Fire()
    {
        // 외부클래스에서 가져오고 싶으면 싱글톤패턴 적용
        Vector3 dir = StageManager.Instance._playerTF.position - _myTF.position;
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
