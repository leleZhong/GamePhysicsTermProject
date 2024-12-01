using UnityEngine;

public class PlanePath : MonoBehaviour
{
    [Header("Circular Path Settings")]
    public Transform centerPoint; // 원의 중심점
    public float radius = 5.0f;   // 원의 반지름
    public float speed = 2.0f;    // 각도 증가 속도 (라디안/초)
    public float escapeSpeed = 5.0f; // 원에서 벗어나는 속도

    private float angle = 0.0f;   // 현재 각도 (라디안)
    private bool isExiting = false; // 원을 벗어나는 상태인지 확인

    void Update()
    {
        if (!isExiting)
        {
            // 각도 증가
            angle += speed * Time.deltaTime;

            // 궤적 계산 (원의 방정식 사용)
            float x = centerPoint.position.x + radius * Mathf.Cos(angle);
            float y = centerPoint.position.y + radius * Mathf.Sin(angle);

            // 비행기 위치 업데이트
            transform.position = new Vector3(x, y, transform.position.z);

            // 비행기 방향 회전
            Vector3 direction = new Vector3(-Mathf.Sin(angle), Mathf.Cos(angle), 0); // 궤적의 접선 벡터
            transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);

            // 한 바퀴를 그리면 원에서 벗어나기
            if (angle >= 2 * Mathf.PI) // 360도(2π) 회전 후 탈출
            {
                isExiting = true;
            }
        }
        else
        {
            // 원에서 탈출 (현재 방향으로 계속 이동)
            transform.position += transform.up * escapeSpeed * Time.deltaTime;
        }
    }
}
