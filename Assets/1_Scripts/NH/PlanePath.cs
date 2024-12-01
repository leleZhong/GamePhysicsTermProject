using UnityEngine;

public class PlanePath : MonoBehaviour
{
    [Header("Circular Path Settings")]
    public Transform centerPoint; // ���� �߽���
    public float radius = 5.0f;   // ���� ������
    public float speed = 2.0f;    // ���� ���� �ӵ� (����/��)
    public float escapeSpeed = 5.0f; // ������ ����� �ӵ�

    private float angle = 0.0f;   // ���� ���� (����)
    private bool isExiting = false; // ���� ����� �������� Ȯ��

    void Update()
    {
        if (!isExiting)
        {
            // ���� ����
            angle += speed * Time.deltaTime;

            // ���� ��� (���� ������ ���)
            float x = centerPoint.position.x + radius * Mathf.Cos(angle);
            float y = centerPoint.position.y + radius * Mathf.Sin(angle);

            // ����� ��ġ ������Ʈ
            transform.position = new Vector3(x, y, transform.position.z);

            // ����� ���� ȸ��
            Vector3 direction = new Vector3(-Mathf.Sin(angle), Mathf.Cos(angle), 0); // ������ ���� ����
            transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);

            // �� ������ �׸��� ������ �����
            if (angle >= 2 * Mathf.PI) // 360��(2��) ȸ�� �� Ż��
            {
                isExiting = true;
            }
        }
        else
        {
            // ������ Ż�� (���� �������� ��� �̵�)
            transform.position += transform.up * escapeSpeed * Time.deltaTime;
        }
    }
}
