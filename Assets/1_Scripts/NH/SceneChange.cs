using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // ���� Ȱ��ȭ�� Scene �̸� ��������
            Scene currentScene = SceneManager.GetActiveScene();

            // ���� Scene �̸��� �������� switch ����
            switch (currentScene.name)
            {
                case "Tutorial":
                    SceneManager.LoadScene("Stage1");
                        break;
                case "Stage1":
                    SceneManager.LoadScene("Stage2");
                    break;
                case "Stage2":
                    SceneManager.LoadScene("Stage3");
                    break;
                case "Stage3":
                    SceneManager.LoadScene("EndingCredit");
                    break;
                default:
                    Debug.LogError("Unknown scene: " + currentScene.name);
                    break;
            }
        }
    }
}
