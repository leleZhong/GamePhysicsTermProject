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

    // [튜토리얼 패널 UI]
    [Header("[TutorialPanel UI]")]
    public GameObject _alertPanel;

    void Start()
    {
        _loading.gameObject.SetActive(false);
        _alertPanel.SetActive(false);
    }

    public void OnClickTutorial()
    {
        _alertPanel.SetActive(true);
    }

    public void OnClickNo()
    {
        _alertPanel.SetActive(false);
    }

    public void OnClickStart()
    {
        _title.gameObject.SetActive(false);
        _loading.gameObject.SetActive(true);
        StartCoroutine(LoadNextScene());
    }

    // 로딩 텍스트 표시 및 다음 씬으로 이동
    IEnumerator LoadNextScene()
    {
        string message = "Loading...";
        int index = 0;
        float loadingDuration = 3f; // 로딩 지속 시간
        float elapsedTime = 0f; // 경과 시간

        while (elapsedTime < loadingDuration)
        {
            // 로딩 텍스트 순환
            _loading.text = message.Substring(0, (index % message.Length) + 1);
            index++;
            elapsedTime += 0.3f; // 딜레이만큼 경과 시간 증가
            yield return new WaitForSeconds(0.3f);
        }

        // 다음 씬으로 이동
        SceneManager.LoadScene("NextSceneName"); // "NextSceneName"을 실제 씬 이름으로 변경
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
