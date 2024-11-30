using UnityEngine;

public class TutorialPlayerMove : MonoBehaviour
{
    public Animator animator; // Animator ����
    public float speed = 5.0f; // �÷��̾� �̵� �ӵ�

    private Vector2 movementInput;       // �̵� �Է� ��

    void Start()
    {
        // Animator ���� Ȯ��
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    void Update()
    {
        // �̵� �Է� ó��
        movementInput.x = Input.GetAxis("Horizontal"); // �¿� �Է�
        movementInput.y = Input.GetAxis("Vertical");   // ���� �Է�

        // �̵� ó��
        HandleMovement();

        // �̵� �ִϸ��̼� ����
        HandleAnimation();
    }

    /// <summary>
    /// �̵� ó��
    /// </summary>
    void HandleMovement()
    {
        // Transform���� �̵� ó��
        transform.Translate(movementInput * speed * Time.deltaTime);
    }

    /// <summary>
    /// �ִϸ��̼� ó��
    /// </summary>
    void HandleAnimation()
    {
        // Animator �Ķ���Ϳ� �� ����
        animator.SetFloat("Horizontal", movementInput.x);
        animator.SetFloat("Vertical", movementInput.y);
        animator.SetFloat("Speed", movementInput.sqrMagnitude); // ������ ���� Ȯ��
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // �浹 �� ĳ���� ����
        if (collision.collider.enabled) // Collider�� Ȱ��ȭ�� ��쿡�� ����
        {
            Destroy(gameObject);
        }
    }
}
