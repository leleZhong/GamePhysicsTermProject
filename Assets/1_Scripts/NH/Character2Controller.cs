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
    public float spellNoEffectDuration = 2.0f; // Spell-NoEffect ���� �ð�

    private float attackCool = 1.0f;
    private float lastAttackTime;
    private float currentBlendValue; //BlendTree ��
    public float currentHP;
    private bool isCasting = false; // Cast ���� ����
    private bool isHurt = false; // Hurt ���� ����
    private bool isDead = false;
    private bool isSpellNoEffect = false;

    public GameObject assaignPlayer;

    void Start()
    {
        currentHP = maxHP; // ü�� �ʱ�ȭ
        assaignPlayer = GameObject.FindGameObjectWithTag("Player");
        target = assaignPlayer.transform;
       

    }

    void Update()
    {

        if (isSpellNoEffect || isDead || isCasting) return; // Ư�� ���¿����� Update ���� �ߴ�


        // Ÿ�ٰ��� �Ÿ� ���
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        // Blend ���� �Ÿ� ������� ����
        if (distanceToTarget > attackRange)
        {
            // Ÿ�ٰ� �ָ� ���� �� Walk ���� ����
            currentBlendValue = Mathf.Lerp(currentBlendValue, 1f, Time.deltaTime * 5); // Walk �ִϸ��̼�
        }
        else
        {
            // Ÿ���� ����� �� Attack ���� ����
            currentBlendValue = Mathf.Lerp(currentBlendValue, 0f, Time.deltaTime * 5); // Attack �ִϸ��̼�
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("bullet") && !isSpellNoEffect && !isDead && !isCasting)
        {
            // Spell-NoEffect ���·� ��ȯ
            //StartCoroutine(TriggerSpellNoEffect());

            // ü�� ���Ҹ� TakeDamage�� ó��
            TakeDamage(10); // �Ѿ˿� ������ 10�� �������� ��
        }
    }



    //private IEnumerator TriggerSpellNoEffect()
    //{
    //    isSpellNoEffect = true; // ���� ����
    //    animator.SetFloat("Blend", currentBlendValue);

    //    // ���� �ð� ���
    //    yield return new WaitForSeconds(spellNoEffectDuration);

    //    // ���� ����
    //    isSpellNoEffect = false;
    //    currentBlendValue = 0.5f; // Walk�� ����
    //    animator.SetFloat("Blend", currentBlendValue);
    //}


    public void TakeDamage(float damage)
    {
        if (isCasting || isDead || isHurt) return; // ���� ���¿��� ������ ����
        animator.SetBool("isWalkAndAttack", false);

        currentHP -= damage;

        // Hurt ���·� ��ȯ
        isHurt = true;
        animator.SetBool("isHurt", true);

        // Hurt ���� ó�� �ڷ�ƾ ����
        StartCoroutine(HurtCooldown());

        // ü���� ���� ������ ��� ĳ���� ����
        if (currentHP <= maxHP / 2 && !isCasting)
        {
            StartCoroutine(StartCast());
        }

        // ü���� 0 ������ ��� ��� ó��
        if (currentHP <= 0 && !isDead)
        {
            isDead = true;
            animator.SetTrigger("Death"); // Death�� ������ Ʈ���� ���
        }
    }


    private IEnumerator HurtCooldown()
    {
        isHurt = true;
        yield return new WaitForSeconds(0.5f); // Hurt �ִϸ��̼� ��� �ð�
        isHurt = false;
        animator.SetBool("isHurt", false);
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