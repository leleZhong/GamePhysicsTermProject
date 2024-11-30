using UnityEngine;

public class GuidedBullet : MonoBehaviour
{
    public GameObject target; // 목표 타겟
    public float speed = 5f; // 탄약 속도
    public float rotateSpeed = 200f; // 회전 속도
    public float hitProbability = 0.3f; // 적중 확률 (30%)
    public int damage = 10; // 탄알 데미지 (기본값)

    private bool willHit = true;
    private Vector2 missDirection; // 빗나가는 방향

    void Awake()
    {
        target = GameObject.Find("Player");
    }

    void Start()
    {
        // 발사 순간 적중 여부 결정
        willHit = Random.value <= hitProbability; // 30% 확률로 true

        // 빗나갈 경우의 방향 설정
        if (!willHit)
        {
            float randomAngle = Random.Range(30f, 150f); // 30도 ~ 150도 사이 랜덤 각도
            randomAngle *= Random.value < 0.5f ? 1 : -1; // 랜덤하게 좌우 방향 결정
            missDirection = Quaternion.Euler(0, 0, randomAngle) * (target.transform.position - transform.position).normalized;
        }
    }

    void Update()
    {
        if (target == null) return;

        Vector2 direction;
        if (willHit)
        {
            // 적중할 경우 정확히 타겟 방향
            direction = (target.transform.position - transform.position).normalized;
        }
        else
        {
            // 빗나갈 경우 미리 설정된 방향으로 이동
            direction = missDirection;
        }

        // 회전 방향 계산
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed * Time.deltaTime);

        // 탄약 이동
        transform.position += transform.right * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform == target && willHit)
        {
            // 타겟 적중 처리
            Debug.Log("Target hit!");
            Destroy(gameObject);
        }
    }
}
