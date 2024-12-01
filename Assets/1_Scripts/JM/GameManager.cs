using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Character3Controller _boss1;
    public Character2Controller _boss2;
    public PlayerController _playerController;
    bool _bossHandled = false;


    [Header("UI Settings")] // UI 관련
    // public GameObject _gameOverMenu; // 게임 오버 UI
    // public GameObject _boomEffect; // 폭발 효과 오브젝트

    [Header("Player Data")]
    int _life; // 남은 생명
    public event Action<int> _onLifeChange; // 생명 변경 이벤트

    public Transform _gemTF; // 젬 TF
    public Transform _gem;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        _life = 5;  // 수정 금지
    }

    void Update()
    {
        if (((_boss1 != null && _boss1.isDead) || (_boss2 != null && _boss2.isDead)) && !_bossHandled)
        {
            HandleBossDeath();
            _bossHandled = true;
        }
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

    void HandleBossDeath()
    {
        if (_playerController != null)
            _playerController.ExpandBoundary();
        Instantiate(_gem, _gemTF);
    }

    public void SetLife(int value)
    {
        _life = value;
        _onLifeChange?.Invoke(_life);
    }
}
