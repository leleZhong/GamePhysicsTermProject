using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // [시작 화면 UI]
    [Header("[Start UI]")]
    public Text _title;
    public Text _loading;

    void Start()
    {
    }

    public void OnClickStart()
    {
        StartCoroutine(LoadNextScene());
    }

    // 로딩 텍스트 표시 및 다음 씬으로 이동
    IEnumerator LoadNextScene()
    {
       
            yield return new WaitForSeconds(0.8f);
        // 다음 씬으로 이동
        SceneManager.LoadScene("Tutorial"); // "NextSceneName"을 실제 씬 이름으로 변경
    }

    public void OnClickExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
