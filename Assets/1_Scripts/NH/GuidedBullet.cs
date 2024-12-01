using UnityEngine;

public class GuidedBullet : MonoBehaviour
{
    public GameObject target; // ��ǥ Ÿ��
    public float speed = 5f; // ź�� �ӵ�
    public float rotateSpeed = 200f; // ȸ�� �ӵ�
    public float hitProbability = 0.3f; // ���� Ȯ�� (30%)
    public int damage = 10; // ź�� ������ (�⺻��)

    private bool willHit = true;
    private Vector2 missDirection; // �������� ����

    void Awake()
    {
        target = GameObject.Find("Player");
    }

    void Start()
    {
        // �߻� ���� ���� ���� ����
        willHit = Random.value <= hitProbability; // 30% Ȯ���� true

        // ������ ����� ���� ����
        if (!willHit)
        {
            float randomAngle = Random.Range(30f, 150f); // 30�� ~ 150�� ���� ���� ����
            randomAngle *= Random.value < 0.5f ? 1 : -1; // �����ϰ� �¿� ���� ����
            missDirection = Quaternion.Euler(0, 0, randomAngle) * (target.transform.position - transform.position).normalized;
        }
    }

    void Update()
    {
        if (target == null) return;

        Vector2 direction;
        if (willHit)
        {
            // ������ ��� ��Ȯ�� Ÿ�� ����
            direction = (target.transform.position - transform.position).normalized;
        }
        else
        {
            // ������ ��� �̸� ������ �������� �̵�
            direction = missDirection;
        }

        // ȸ�� ���� ���
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed * Time.deltaTime);

        // ź�� �̵�
        transform.position += transform.right * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform == target && willHit)
        {
            // Ÿ�� ���� ó��
            Debug.Log("Target hit!");
            Destroy(gameObject);
        }
    }
}
