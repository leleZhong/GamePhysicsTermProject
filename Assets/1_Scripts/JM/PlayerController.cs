using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float _speed;
    public Vector2 _minBoundary; // 최소 경계
    public Vector2 _maxBoundary; // 최대 경계

    [Header("Animation Settings")]
    public Animator _anim;

    [Header("Bullet Settings")]
    public Transform[] _bullet; // 프리팹 배열
    public float _delay; // 발사 딜레이
    public int _playerLevel; // 플레이어 레벨에 따라 변경

    [Header("Transform Settings")]
    public Transform _myTF; // 플레이어 Transform

    [Header("Sprite Settings")]
    public SpriteRenderer _spriteRenderer;

    [Header("Internal States")] // 내부적으로 관리하는 상태 변수
    float _currentTime; // 시간 체크
    bool _isBegine; // 무적 상태 확인
    bool _isRespawning; // 리스폰 중 여부 확인
    WaitForSeconds _wait = new WaitForSeconds(0.1f); // 반복 대기 시간
    Color[] _color; // 무적 상태 시 색상 변화
    
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
        // 이동영역 제한
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
                // 0으로 하지 말고 깎아주는 식으로 해야 정확한 계산 가능
                _currentTime -= _delay;
                Fire();
            }
        }
    }

    void Fire() // 총알 발사 함수
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
        if (_isBegine || _isRespawning)  // 무적 상태면 충돌 무시
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
            StartCoroutine(PlayerBegine()); // 무적 상태 시작
        }
        _isRespawning = false; // 리스폰 중 상태 종료
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
