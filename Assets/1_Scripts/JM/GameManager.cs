using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Settings")] // UI ����
    // public GameObject _gameOverMenu; // ���� ���� UI
    // public GameObject _boomEffect; // ���� ȿ�� ������Ʈ

    [Header("Player Data")]
    public int _life; // ���� ����
    public event Action<int> _onLifeChange; // ���� ���� �̺�Ʈ

    void Awake()
    {
        Instance = this;

        // _gameOverMenu.SetActive(false);
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
