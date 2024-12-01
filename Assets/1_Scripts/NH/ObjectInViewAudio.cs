using UnityEngine;

public class ObjectInViewAudio : MonoBehaviour
{
    public GameObject targetObject; // 카메라 시야에 들어오는지 확인할 대상
    public AudioSource audioSource; // 재생할 AudioSource
    private Camera mainCamera; // 메인 카메라
    public AudioClip Appearclip;
    private bool hasPlayed = false; // 소리가 이미 재생되었는지 확인하는 플래그

    void Start()
    {
        // 메인 카메라 가져오기
        mainCamera = Camera.main;

        // AudioSource가 설정되지 않았을 경우 자동으로 가져오기
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        // audioSource가 없으면 경고 출력
        if (audioSource == null)
        {
            Debug.LogError("AudioSource is not assigned!");
        }
    }

    void Update()
    {
        if (hasPlayed) return;

        if (IsObjectInView(targetObject))
        {
            
                audioSource.clip = Appearclip;
                audioSource.loop = false;
                audioSource.Play();
                hasPlayed = true;
          
        }
        else
        {
            //if (audioSource.isPlaying) // 소리가 재생 중이면 중지
            //{
            //    audioSource.Stop();
            //}
        }
    }

    // 카메라의 뷰포트 내에 물체가 있는지 확인하는 함수
    private bool IsObjectInView(GameObject obj)
    {
        if (obj == null || mainCamera == null) return false;

        // 물체의 위치를 카메라의 뷰포트 좌표로 변환
        Vector3 viewportPos = mainCamera.WorldToViewportPoint(obj.transform.position);

        // 뷰포트 내에 있는지 확인 (x, y는 0~1 사이, z > 0이면 카메라 앞)
        return viewportPos.x > 0 && viewportPos.x < 1 &&
               viewportPos.y > 0 && viewportPos.y < 1 &&
               viewportPos.z > 0;
    }
}
