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

    void Start()
    {
    }

    public void OnClickStart()
    {
        StartCoroutine(LoadNextScene());
    }

    // �ε� �ؽ�Ʈ ǥ�� �� ���� ������ �̵�
    IEnumerator LoadNextScene()
    {
       
            yield return new WaitForSeconds(0.8f);
        // ���� ������ �̵�
        SceneManager.LoadScene("Tutorial"); // "NextSceneName"�� ���� �� �̸����� ����
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
