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
    public float spellNoEffectDuration = 2.0f; // Spell-NoEffect 유지 시간

    private float attackCool = 1.0f;
    private float lastAttackTime;
    private float currentBlendValue; //BlendTree 값
    public float currentHP;
    private bool isCasting = false; // Cast 상태 여부
    private bool isHurt = false; // Hurt 상태 여부
    private bool isDead = false;
    private bool isSpellNoEffect = false;

    public GameObject assaignPlayer;

    void Start()
    {
        currentHP = maxHP; // 체력 초기화
        assaignPlayer = GameObject.FindGameObjectWithTag("Player");
        target = assaignPlayer.transform;
       

    }

    void Update()
    {

        if (isSpellNoEffect || isDead || isCasting) return; // 특정 상태에서는 Update 로직 중단


        // 타겟과의 거리 계산
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        // Blend 값을 거리 기반으로 설정
        if (distanceToTarget > attackRange)
        {
            // 타겟과 멀리 있을 때 Walk 비중 높임
            currentBlendValue = Mathf.Lerp(currentBlendValue, 1f, Time.deltaTime * 5); // Walk 애니메이션
        }
        else
        {
            // 타겟이 가까울 때 Attack 비중 높임
            currentBlendValue = Mathf.Lerp(currentBlendValue, 0f, Time.deltaTime * 5); // Attack 애니메이션
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("bullet") && !isSpellNoEffect && !isDead && !isCasting)
        {
            // Spell-NoEffect 상태로 전환
            //StartCoroutine(TriggerSpellNoEffect());

            // 체력 감소를 TakeDamage로 처리
            TakeDamage(10); // 총알에 맞으면 10의 데미지를 줌
        }
    }



    //private IEnumerator TriggerSpellNoEffect()
    //{
    //    isSpellNoEffect = true; // 상태 고정
    //    animator.SetFloat("Blend", currentBlendValue);

    //    // 일정 시간 대기
    //    yield return new WaitForSeconds(spellNoEffectDuration);

    //    // 상태 복원
    //    isSpellNoEffect = false;
    //    currentBlendValue = 0.5f; // Walk로 복귀
    //    animator.SetFloat("Blend", currentBlendValue);
    //}


    public void TakeDamage(float damage)
    {
        if (isCasting || isDead || isHurt) return; // 현재 상태에서 데미지 무시
        animator.SetBool("isWalkAndAttack", false);

        currentHP -= damage;

        // Hurt 상태로 전환
        isHurt = true;
        animator.SetBool("isHurt", true);

        // Hurt 상태 처리 코루틴 시작
        StartCoroutine(HurtCooldown());

        // 체력이 절반 이하일 경우 캐스팅 시작
        if (currentHP <= maxHP / 2 && !isCasting)
        {
            StartCoroutine(StartCast());
        }

        // 체력이 0 이하일 경우 사망 처리
        if (currentHP <= 0 && !isDead)
        {
            isDead = true;
            animator.SetTrigger("Death"); // Death는 여전히 트리거 사용
        }
    }


    private IEnumerator HurtCooldown()
    {
        isHurt = true;
        yield return new WaitForSeconds(0.5f); // Hurt 애니메이션 재생 시간
        isHurt = false;
        animator.SetBool("isHurt", false);
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