using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoTrimmer : MonoBehaviour
{
    public VideoPlayer videoPlayer;      // VideoPlayer 컴포넌트
    public string nextSceneName = "Stage1"; // 변경할 씬 이름

    private bool sceneChangeTriggered = false; // 씬 변경 여부를 확인

    void Start()
    {
        videoPlayer.Play(); // 비디오 재생
        videoPlayer.loopPointReached += OnVideoEnd; // 비디오 종료 시 이벤트 연결
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        if (!sceneChangeTriggered) // 씬 변경이 중복되지 않도록 확인
        {
            sceneChangeTriggered = true;
            Invoke("ChangeScene", 0.1f); // 1초 후 씬 변경
        }
    }

    void ChangeScene()
    {
        SceneManager.LoadScene(nextSceneName); // 지정한 씬으로 이동
    }
}
