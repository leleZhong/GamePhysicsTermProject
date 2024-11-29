using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // [���� ȭ�� UI]
    [Header("[Start UI]")]
    public Text _title;
    public Text _loading;

    // [Ʃ�丮�� �г� UI]
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

    // �ε� �ؽ�Ʈ ǥ�� �� ���� ������ �̵�
    IEnumerator LoadNextScene()
    {
        string message = "Loading...";
        int index = 0;
        float loadingDuration = 3f; // �ε� ���� �ð�
        float elapsedTime = 0f; // ��� �ð�

        while (elapsedTime < loadingDuration)
        {
            // �ε� �ؽ�Ʈ ��ȯ
            _loading.text = message.Substring(0, (index % message.Length) + 1);
            index++;
            elapsedTime += 0.3f; // �����̸�ŭ ��� �ð� ����
            yield return new WaitForSeconds(0.3f);
        }

        // ���� ������ �̵�
        SceneManager.LoadScene("NextSceneName"); // "NextSceneName"�� ���� �� �̸����� ����
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
