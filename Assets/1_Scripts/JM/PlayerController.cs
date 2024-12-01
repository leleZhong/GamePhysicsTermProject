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
    float _currentTime; // �ð� üũ
    bool _isBegine; // ���� ���� Ȯ��
    bool _isRespawning; // ������ �� ���� Ȯ��
    WaitForSeconds _wait = new WaitForSeconds(0.1f); // �ݺ� ��� �ð�
    Color[] _color; // ���� ���� �� ���� ��ȭ
    
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
        switch (_playerLevel)
        {
            case 0:
                PoolManager.Spawn(_bullet[0].gameObject, _myTF.position, _myTF.rotation);
                break;
            case 1:
                PoolManager.Spawn(_bullet[0].gameObject, _myTF.position + new Vector3(0, 0.1f, 0), _myTF.rotation);
                PoolManager.Spawn(_bullet[0].gameObject, _myTF.position - new Vector3(0, 0.1f, 0), _myTF.rotation);
                break;
            case 2:
                PoolManager.Spawn(_bullet[0].gameObject, _myTF.position + new Vector3(0, 0.25f, 0) + new Vector3(0, 0.1f, 0), _myTF.rotation);
                PoolManager.Spawn(_bullet[0].gameObject, _myTF.position - new Vector3(0, 0.25f, 0) - new Vector3(0, 0.1f, 0), _myTF.rotation);
                PoolManager.Spawn(_bullet[1].gameObject, _myTF.position, _myTF.rotation);
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (_isBegine || _isRespawning)  // ���� ���¸� �浹 ����
            return;

        switch (other.tag)
        {
            case "Enemy":
            case "enemyBullet":
            case "Boss":
            _isRespawning = true;
                gameObject.SetActive(false);
                Destroy(other.gameObject);
                Invoke("ShowPlayer", 2);
                break;
            case "item":
                break;
        }
    }

    void ShowPlayer()
    {
        if (GameManager.Instance.RespawnPlayer())
        {
            gameObject.SetActive(true);
            _myTF.position = new Vector3(-7, 0, 0);
            StartCoroutine(PlayerBegine()); // ���� ���� ����
        }
        _isRespawning = false; // ������ �� ���� ����
    }

    IEnumerator PlayerBegine()
    {
        _isBegine = true;
        
        for (int i = 0; i < 20; i++)
        {
            _spriteRenderer.color = _color[0];
            yield return _wait;
            _spriteRenderer.color = _color[1];
            yield return _wait;
        }
        _isBegine = false;
    }

    public void ExpandBoundary()
    {
        _maxBoundary += new Vector2(8, 0);
    }
}
