using UnityEngine;

public class ObjectInViewAudio : MonoBehaviour
{
    public GameObject targetObject; // ī�޶� �þ߿� �������� Ȯ���� ���
    public AudioSource audioSource; // ����� AudioSource
    private Camera mainCamera; // ���� ī�޶�
    public AudioClip Appearclip;
    private bool hasPlayed = false; // �Ҹ��� �̹� ����Ǿ����� Ȯ���ϴ� �÷���

    void Start()
    {
        // ���� ī�޶� ��������
        mainCamera = Camera.main;

        // AudioSource�� �������� �ʾ��� ��� �ڵ����� ��������
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        // audioSource�� ������ ��� ���
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
            //if (audioSource.isPlaying) // �Ҹ��� ��� ���̸� ����
            //{
            //    audioSource.Stop();
            //}
        }
    }

    // ī�޶��� ����Ʈ ���� ��ü�� �ִ��� Ȯ���ϴ� �Լ�
    private bool IsObjectInView(GameObject obj)
    {
        if (obj == null || mainCamera == null) return false;

        // ��ü�� ��ġ�� ī�޶��� ����Ʈ ��ǥ�� ��ȯ
        Vector3 viewportPos = mainCamera.WorldToViewportPoint(obj.transform.position);

        // ����Ʈ ���� �ִ��� Ȯ�� (x, y�� 0~1 ����, z > 0�̸� ī�޶� ��)
        return viewportPos.x > 0 && viewportPos.x < 1 &&
               viewportPos.y > 0 && viewportPos.y < 1 &&
               viewportPos.z > 0;
    }
}
