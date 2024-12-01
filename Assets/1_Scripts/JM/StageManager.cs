using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance;

    [Header("Stage Settings")]
    int _score;
    public event Action<int> _onScoreChange;
    bool _isBossSpawned = false;    // 보스 등장 체크

    [Header("Enemy Settings")]  // 적 관련 설정
    public Transform[] _enemy; // 적 프리팹 배열
    public Transform[] _myTF; // 적 생성 위치 배열
    public Transform _playerTF; // 플레이어 Transform
    public float _delay; // 적 생성 딜레이
    float _currentTime; // 내부 시간 확인

    [Header("Boss Settings")]
    public GameObject _boss;

    [Header("Target Object")]
    public GameObject targetObject; // 카메라 시야 확인할 대상 오브젝트
    Camera _mainCamera;

    void Awake()
    {
        Instance = this;

        _score = 0;

        _mainCamera = Camera.main;
    }

    void Start()
    {
        _currentTime = Time.time + _delay + UnityEngine.Random.Range(2, 4f);
    }

    void Update()
    {
        if (_currentTime < Time.time && !_isBossSpawned)
        {
            Generate();
            _currentTime = Time.time + _delay + UnityEngine.Random.Range(0.5f, 2f);
        }

        if (SceneManager.GetActiveScene().name != "Stage2" && _score >= 50 && !_isBossSpawned)
        {
            // 보스 프리팹 생성
            SpawnBoss();
        }

         // 특정 오브젝트가 카메라 시야에 있을 때 보스 스폰 상태로 전환
        if (targetObject != null && IsObjectInView(targetObject))
        {
            _isBossSpawned = true;
        }
    }

    void SpawnBoss()
    {
        _boss.SetActive(true);
        _isBossSpawned = true;

        Debug.Log("보스 등장!");
    }

    void Generate()
    {
        Transform obj = _myTF[UnityEngine.Random.Range(0, _myTF.Length)];
        Vector3 dir = _playerTF.position - obj.position;
        Quaternion rotate = Quaternion.FromToRotation(Vector3.down, dir);

        int num = UnityEngine.Random.Range(0, _enemy.Length);
        Instantiate(_enemy[num], obj.position, rotate);
    }

    public void AddScore(int num)
    {
        _score += num;
        _onScoreChange?.Invoke(_score);
    }

    bool IsObjectInView(GameObject obj)
    {
        if (obj == null || _mainCamera == null) return false;

        // 오브젝트의 월드 좌표를 뷰포트 좌표로 변환
        Vector3 viewportPos = _mainCamera.WorldToViewportPoint(obj.transform.position);

        // 뷰포트 내에 있는지 확인 (x, y는 0~1 사이, z > 0이면 카메라 앞)
        return viewportPos.x > 0 && viewportPos.x < 1 &&
               viewportPos.y > 0 && viewportPos.y < 1 &&
               viewportPos.z > 0;
    }
}
