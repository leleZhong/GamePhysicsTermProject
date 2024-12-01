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
            // 현재 활성화된 Scene 이름 가져오기
            Scene currentScene = SceneManager.GetActiveScene();

            // 현재 Scene 이름을 바탕으로 switch 실행
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
