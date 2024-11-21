using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float _speed;
    public Vector2 _minBoundary; // �ּ� ���
    public Vector2 _maxBoundary; // �ִ� ���

    [Header("Animation Settings")]
    public Animator _anim;

    [Header("Bullet Settings")]
    public Transform[] _bullet; // ������ �迭
    public float _delay; // �߻� ������
    public int _playerLevel; // �÷��̾� ������ ���� ����

    [Header("Transform Settings")]
    public Transform _myTF; // �÷��̾� Transform

    [Header("Sprite Settings")]
    public SpriteRenderer _spriteRenderer;

    [Header("Internal States")] // ���������� �����ϴ� ���� ����
    private float _currentTime; // �ð� üũ
    private bool _isBegine; // ���� ���� Ȯ��
    private WaitForSeconds _wait = new WaitForSeconds(0.1f); // �ݺ� ��� �ð�
    private Color[] _color; // ���� ���� �� ���� ��ȭ
    
    void Awake()
    {
        _color = new Color[2];
        _color[0] = new Color(1, 1, 1, 0.1f);
        _color[1] = new Color(1, 1, 1, 1);
    }
    
    void Start()
    {
        _currentTime = 0;
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 nextPos = new Vector3(h, v, 0) * _speed * Time.deltaTime;
        Vector3 currentPos = _myTF.position + nextPos;
        // �̵����� ����
        currentPos.x = Mathf.Clamp(currentPos.x, _minBoundary.x, _maxBoundary.x);
        currentPos.y = Mathf.Clamp(currentPos.y, _minBoundary.y, _maxBoundary.y);

        _myTF.position = currentPos;

        if(Input.GetButtonDown("Vertical") || Input.GetButtonUp("Vertical"))
        {
            _anim.SetInteger("Input", (int)v);
        }

        if(Input.GetButton("Fire1"))
        {
            _currentTime += Time.deltaTime;
            if(_currentTime >= _delay)
            {
                // 0���� ���� ���� ����ִ� ������ �ؾ� ��Ȯ�� ��� ����
                _currentTime -= _delay;
                Fire();
            }
        }
    }

    void Fire() // �Ѿ� �߻� �Լ�
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (_isBegine)
            return;
        switch (other.tag)
        {
            case "enemy":
                break;
            case "enemyBullet":
                break;
            case "item":
                break;
        }
    }
}
