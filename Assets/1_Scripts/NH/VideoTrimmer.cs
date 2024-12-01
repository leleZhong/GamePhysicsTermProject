using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoTrimmer : MonoBehaviour
{
    public VideoPlayer videoPlayer;      // VideoPlayer ������Ʈ
    public string nextSceneName = "Stage1"; // ������ �� �̸�

    private bool sceneChangeTriggered = false; // �� ���� ���θ� Ȯ��

    void Start()
    {
        videoPlayer.Play(); // ���� ���
        videoPlayer.loopPointReached += OnVideoEnd; // ���� ���� �� �̺�Ʈ ����
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        if (!sceneChangeTriggered) // �� ������ �ߺ����� �ʵ��� Ȯ��
        {
            sceneChangeTriggered = true;
            Invoke("ChangeScene", 0.1f); // 1�� �� �� ����
        }
    }

    void ChangeScene()
    {
        SceneManager.LoadScene(nextSceneName); // ������ ������ �̵�
    }
}
