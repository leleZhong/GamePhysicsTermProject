using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage3 : MonoBehaviour
{
    public static Stage3 Instance;

    [Header("Enemy Settings")]  // 적 관련 설정
    public Transform[] _enemy; // 적 프리팹 배열
    public Transform[] _myTF; // 적 생성 위치 배열
    public Transform _playerTF; // 플레이어 Transform
    public float _delay; // 적 생성 딜레이
    float _currentTime; // 내부 시간 확인

    void Awake()
    {
        Instance = this;
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
    }

    void Generate()
    {
        Transform obj = _myTF[UnityEngine.Random.Range(0, _myTF.Length)];
        Vector3 dir = _playerTF.position - obj.position;
        Quaternion rotate = Quaternion.FromToRotation(Vector3.down, dir);

        int num = UnityEngine.Random.Range(0, _enemy.Length);
        Instantiate(_enemy[num], obj.position, rotate);
    }
}
