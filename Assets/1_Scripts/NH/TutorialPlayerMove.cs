using UnityEngine;

public class TutorialPlayerMove : MonoBehaviour
{
    public Animator animator; // Animator 연결
    public float speed = 5.0f; // 플레이어 이동 속도

    private Vector2 movementInput;       // 이동 입력 값

    void Start()
    {
        // Animator 연결 확인
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    void Update()
    {
        // 이동 입력 처리
        movementInput.x = Input.GetAxis("Horizontal"); // 좌우 입력
        movementInput.y = Input.GetAxis("Vertical");   // 상하 입력

        // 이동 처리
        HandleMovement();

        // 이동 애니메이션 설정
        HandleAnimation();
    }

    /// <summary>
    /// 이동 처리
    /// </summary>
    void HandleMovement()
    {
        // Transform으로 이동 처리
        transform.Translate(movementInput * speed * Time.deltaTime);
    }

    /// <summary>
    /// 애니메이션 처리
    /// </summary>
    void HandleAnimation()
    {
        // Animator 파라미터에 값 전달
        animator.SetFloat("Horizontal", movementInput.x);
        animator.SetFloat("Vertical", movementInput.y);
        animator.SetFloat("Speed", movementInput.sqrMagnitude); // 움직임 여부 확인
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌 시 캐릭터 제거
        if (collision.collider.enabled) // Collider가 활성화된 경우에만 동작
        {
            Destroy(gameObject);
        }
    }
}
