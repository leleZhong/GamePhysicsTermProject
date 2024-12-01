using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float _speed; // �̵� �ӵ�
    public Rigidbody2D _rd;
    public Transform _myTF;
    public Animator animator;


    [Header("Attack Settings")]
    public Transform _bullet;
    public bool _isShooting;
    float _currentTime; // �߻� ���� Ÿ�̸�

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
        // �ܺ�Ŭ�������� �������� ������ �̱������� ����
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
