using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Character2Controller : MonoBehaviour
{
    public Transform target; // ĳ����1�� Transform
    public float speed = 2.0f; // �̵� �ӵ�
    public float maxHP = 100f; // �ִ� ü��
    public float attackRange = 3.0f; // ���� �Ÿ�
    public Animator animator;

    private float attackCool = 1.0f;
    private float lastAttackTime;
    private float currentBlendValue; //BlendTree ��
    private float currentHP;
    private bool isCasting = false; // Cast ���� ����
    private bool isHurt = false; // Hurt ���� ����
    private bool isDead = false;

    public GameObject assaignPlayer;

    void Start()
    {
        currentHP = maxHP; // ü�� �ʱ�ȭ
        assaignPlayer = GameObject.FindGameObjectWithTag("Player");
        target = assaignPlayer.transform;

    }

    void Update()
    {
        // Ÿ�ٰ��� �Ÿ� ���
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        // Blend ���� �Ÿ� ������� ����
        if (distanceToTarget > attackRange)
        {
            // Ÿ�ٰ� �ָ� ���� �� Walk ���� ����
            currentBlendValue = Mathf.Lerp(currentBlendValue, 0.5f, Time.deltaTime * 5); // Walk �ִϸ��̼�
        }
        else
        {
            // Ÿ���� ����� �� Attack ���� ����
            currentBlendValue = Mathf.Lerp(currentBlendValue, 0f, Time.deltaTime * 5); // Attack �ִϸ��̼�
        }

       void OnTriggerEnter2D(Collider other)
        {
            if(other.tag == "bullet")
            {
                currentBlendValue = Mathf.Lerp(currentBlendValue, 1f, Time.deltaTime * 5);
            }
        }

        // Blend ���� Animator�� ����
        animator.SetFloat("Blend", currentBlendValue);

        // Walk ����(Ÿ���� ���� �̵�)
        if (distanceToTarget > attackRange)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // WalkAndAttack �ִϸ��̼� ����
            animator.SetBool("isWalkAndAttack", true);
        }

        // ���� ��Ÿ�� Ȯ��
        //if (Time.time - lastAttackTime >= attackCool && distanceToTarget <= attackRange)
        //{
        //    // ���� ����
        //    lastAttackTime = Time.time;
        //    animator.SetTrigger("AttackTrigger"); // Attack �ִϸ��̼� Ʈ����
        //}
    }

    public void TakeDamage(float damage)
    {
        if (isCasting) return; // Cast ���¿����� �������� ���� ����

        currentHP -= damage;
        animator.SetBool("isWalkAndAttack", false);
        animator.SetTrigger("Hurt"); // Hurt �ִϸ��̼� ����
        StartCoroutine(HurtCooldown()); // Hurt ���� ó��

        if (currentHP <= maxHP / 2 && !isCasting)
        {
            StartCoroutine(StartCast()); // ü���� ���� ������ ��� Cast ���·� ��ȯ
        }

        if (currentHP <= 0 && !isDead)
        {
            isDead = true; // ��� ó��
            animator.SetTrigger("Death");
        }
    }

    private IEnumerator HurtCooldown()
    {
        isHurt = true;
        yield return new WaitForSeconds(0.5f); // Hurt �ִϸ��̼� ��� �ð�
        isHurt = false;
    }

    private IEnumerator StartCast()
    {
        isCasting = true; //ĳ���� �ϰ��ִµ��� �������� �ʵ���

        // Hurt-NoEffect �ִϸ��̼� ����
        animator.SetTrigger("HurtNoEffect");
        yield return new WaitForSeconds(3.0f); // Hurt-NoEffect �ִϸ��̼� ��� �ð�

        // Cast �ִϸ��̼� ����
        animator.SetTrigger("Cast");

        yield return new WaitForSeconds(1.0f); // Cast ���� Flame ����
        animator.SetTrigger("Flame");

        // Flame�� �Ϸ�Ǹ� Spell ����
        yield return new WaitForSeconds(1.0f); // Flame �ִϸ��̼� ��� �ð�
        animator.SetTrigger("Spell");



        // �ٽ� �̵� ����
        isCasting = false;
    }
}