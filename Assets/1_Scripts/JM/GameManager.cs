using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Enemy Settings")]  // 적 관련 설정
    public Transform[] _enemy; // 적 프리팹 배열
    public Transform[] _myTF; // 적 생성 위치 배열
    public Transform _playerTF; // 플레이어 Transform
    public float _delay; // 적 생성 딜레이
    float _currentTime; // 내부 시간 확인

    [Header("UI Settings")] // UI 관련
    // public GameObject _gameOverMenu; // 게임 오버 UI
    // public GameObject _boomEffect; // 폭발 효과 오브젝트

    [Header("Player Data")]
    int _life; // 남은 생명
    public event Action<int> _onLifeChange; // 생명 변경 이벤트

    void Awake()
    {
        Instance = this;

        // _gameOverMenu.SetActive(false);
        _life = 3;
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

    public bool RespawnPlayer()
    {
        _life--;
        if(_life < 0)
        {
            // _gameOverMenu.SetActive(true);
            return false;
        }
        else
        {
            _onLifeChange?.Invoke(_life);
            return true;
        }
    }

    public void ButtonAct_Restart()
    {
        SceneManager.LoadScene(0);
    }
}
