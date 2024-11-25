using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Character2Controller : MonoBehaviour
{
    public Transform target; // 캐릭터1의 Transform
    public float speed = 2.0f; // 이동 속도
    public float maxHP = 100f; // 최대 체력
    public float attackRange = 3.0f; // 공격 거리
    public Animator animator;

    private float attackCool = 1.0f;
    private float lastAttackTime;
    private float currentBlendValue; //BlendTree 값
    private float currentHP;
    private bool isCasting = false; // Cast 상태 여부
    private bool isHurt = false; // Hurt 상태 여부
    private bool isDead = false;

    public GameObject assaignPlayer;

    void Start()
    {
        currentHP = maxHP; // 체력 초기화
        assaignPlayer = GameObject.FindGameObjectWithTag("Player");
        target = assaignPlayer.transform;

    }

    void Update()
    {
        // 타겟과의 거리 계산
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        // Blend 값을 거리 기반으로 설정
        if (distanceToTarget > attackRange)
        {
            // 타겟과 멀리 있을 때 Walk 비중 높임
            currentBlendValue = Mathf.Lerp(currentBlendValue, 0.5f, Time.deltaTime * 5); // Walk 애니메이션
        }
        else
        {
            // 타겟이 가까울 때 Attack 비중 높임
            currentBlendValue = Mathf.Lerp(currentBlendValue, 0f, Time.deltaTime * 5); // Attack 애니메이션
        }

       void OnTriggerEnter2D(Collider other)
        {
            if(other.tag == "bullet")
            {
                currentBlendValue = Mathf.Lerp(currentBlendValue, 1f, Time.deltaTime * 5);
            }
        }

        // Blend 값을 Animator에 전달
        animator.SetFloat("Blend", currentBlendValue);

        // Walk 동작(타겟을 향해 이동)
        if (distanceToTarget > attackRange)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // WalkAndAttack 애니메이션 실행
            animator.SetBool("isWalkAndAttack", true);
        }

        // 공격 쿨타임 확인
        //if (Time.time - lastAttackTime >= attackCool && distanceToTarget <= attackRange)
        //{
        //    // 공격 실행
        //    lastAttackTime = Time.time;
        //    animator.SetTrigger("AttackTrigger"); // Attack 애니메이션 트리거
        //}
    }

    public void TakeDamage(float damage)
    {
        if (isCasting) return; // Cast 상태에서는 데미지를 받지 않음

        currentHP -= damage;
        animator.SetBool("isWalkAndAttack", false);
        animator.SetTrigger("Hurt"); // Hurt 애니메이션 실행
        StartCoroutine(HurtCooldown()); // Hurt 상태 처리

        if (currentHP <= maxHP / 2 && !isCasting)
        {
            StartCoroutine(StartCast()); // 체력이 절반 이하일 경우 Cast 상태로 전환
        }

        if (currentHP <= 0 && !isDead)
        {
            isDead = true; // 사망 처리
            animator.SetTrigger("Death");
        }
    }

    private IEnumerator HurtCooldown()
    {
        isHurt = true;
        yield return new WaitForSeconds(0.5f); // Hurt 애니메이션 재생 시간
        isHurt = false;
    }

    private IEnumerator StartCast()
    {
        isCasting = true; //캐스팅 하고있는동안 움직이지 않도록

        // Hurt-NoEffect 애니메이션 실행
        animator.SetTrigger("HurtNoEffect");
        yield return new WaitForSeconds(3.0f); // Hurt-NoEffect 애니메이션 재생 시간

        // Cast 애니메이션 실행
        animator.SetTrigger("Cast");

        yield return new WaitForSeconds(1.0f); // Cast 이후 Flame 실행
        animator.SetTrigger("Flame");

        // Flame이 완료되면 Spell 실행
        yield return new WaitForSeconds(1.0f); // Flame 애니메이션 재생 시간
        animator.SetTrigger("Spell");



        // 다시 이동 시작
        isCasting = false;
    }
}