using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject[] _life;

    void OnEnable()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance is null");
            return;
        }

        GameManager.Instance._onLifeChange += OnLifeChange;
    }

    void OnDisable()
    {
        GameManager.Instance._onLifeChange -= OnLifeChange;
    }

    void OnLifeChange(int life)
    {
        for (int i = 0; i < _life.Length; i++)
        {
            if (i < life)
            {
                _life[i].SetActive(true);
            }
            else
            {
                _life[i].SetActive(false);
            }
        }
    }
}
