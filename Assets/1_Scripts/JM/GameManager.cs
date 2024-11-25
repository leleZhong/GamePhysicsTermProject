using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Enemy Settings")]  // �� ���� ����
    public Transform[] _enemy; // �� ������ �迭
    public Transform[] _myTF; // �� ���� ��ġ �迭
    public Transform _playerTF; // �÷��̾� Transform
    public float _delay; // �� ���� ������
    float _currentTime; // ���� �ð� Ȯ��

    [Header("UI Settings")] // UI ����
    // public GameObject _gameOverMenu; // ���� ���� UI
    // public GameObject _boomEffect; // ���� ȿ�� ������Ʈ

    [Header("Player Data")]
    int _life; // ���� ����
    public event Action<int> _onLifeChange; // ���� ���� �̺�Ʈ

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
