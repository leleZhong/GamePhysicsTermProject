using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Transform _bossPrefab; // 보스 프리팹
    private List<GameObject> _spawnedEnemies = new List<GameObject>();

    void Awake()
    {
        Instance = this;

        _score = 0;
    }

    void Start()
    {
        _currentTime = Time.time + _delay + UnityEngine.Random.Range(2, 4f);
    }

    void Update()
    {
        if (_currentTime < Time.time)
        {
            Generate();
            _currentTime = Time.time + _delay + UnityEngine.Random.Range(0.5f, 2f);
        }

        if (_score >= 50 && !_isBossSpawned)
        {
            // 보스 프리팹 생성
            SpawnBoss();
        }
    }

    void SpawnBoss()
    {
        // 잡몹 제거
        foreach (GameObject enemy in _spawnedEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }
        _spawnedEnemies.Clear(); // 적 리스트 초기화

        // 보스 생성
        Instantiate(_bossPrefab);
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
}
