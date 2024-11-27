using System.Collections;
using UnityEngine;

public class Character2Controller : MonoBehaviour
{
    public Transform target;
    public float speed = 2.0f;
    public float maxHP = 100f;
    public float attackRange = 3.0f;
    public Animator animator;
    public float spellNoEffectDuration = 2.0f;
    public GameObject Fire;

    private float currentBlendValue = 0.5f;
    public float currentHP;
    private bool isCasting = false;
    private bool isHurt = false;
    private bool isDead = false;
    private bool hasCastSpell = false;
    private bool isSpellNoEffect = false;

    public BackGround background;

    public GameObject assignedPlayer;

    private SpriteRenderer spriteRenderer;
    private Color originalColor; // 원래 색상 저장

    void Start()
    {
        currentHP = maxHP;
        assignedPlayer = GameObject.FindGameObjectWithTag("Player");
        target = assignedPlayer.transform;

        // SpriteRenderer 초기화
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color; // 원래 색상 저장
        }
    }

    void Update()
    {
        if (isSpellNoEffect || isDead || isCasting || isHurt) return;

        float distanceToTarget = Vector3.Distance(transform.position, new Vector3(target.position.x, transform.position.y, target.position.z));

        if (distanceToTarget > attackRange)
        {
            currentBlendValue = Mathf.Lerp(currentBlendValue, 1f, Time.deltaTime * 5);
        }
        else
        {
            currentBlendValue = Mathf.Lerp(currentBlendValue, 0f, Time.deltaTime * 5);
            //SetBackgroundSpeed(0);

            // Spell이 실행되지 않은 경우에만 MoveAfterDelay 실행
            if (!hasCastSpell)
            {
                StartCoroutine(MoveAfterDelay());
            }


        }

        animator.SetFloat("Blend", currentBlendValue);

        if (distanceToTarget > attackRange)
        {
            Vector3 direction = new Vector3(target.position.x - transform.position.x,0f,0f).normalized;
            transform.position += direction * speed * Time.deltaTime;
            animator.SetBool("isWalkAndAttack", true);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("bullet") && !isSpellNoEffect && !isDead && !isCasting && !isHurt)
        {
            TakeDamage(10);
        }
    }

    private void SetBackgroundSpeed(float speed)
    {
        // Hierarchy의 BackGround 아래에 있는 모든 BackGround 스크립트를 가져옴
        BackGround[] backgrounds = FindObjectsOfType<BackGround>();

        foreach (BackGround bg in backgrounds)
        {
            bg._speed = speed; // speed 값 변경
        }
    }

    public void TakeDamage(float damage)
    {
        if (isCasting || isDead || isHurt) return;

        currentHP -= damage;
        isHurt = true;
        animator.SetBool("isHurt", true);

        StartCoroutine(HurtCooldown());

        if (currentHP == maxHP / 2 && !isCasting)
        {
            StartCoroutine(StartCast());
        }

        if (currentHP <= 0 && !isDead)
        {
            isDead = true;
            animator.SetTrigger("Death");
            SetBackgroundSpeed(0);

        }
    }

    private IEnumerator HurtCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        isHurt = false;
        animator.SetBool("isHurt", false);
    }

    private IEnumerator StartCast()
    {
        isCasting = true;

        animator.SetTrigger("HurtNoEffect");
        yield return new WaitForSeconds(3.0f);

        animator.SetTrigger("Cast");
        yield return new WaitForSeconds(1.0f);

        animator.SetTrigger("Flame");
        yield return new WaitForSeconds(1.0f);

        animator.SetTrigger("Spell");

        yield return new WaitForSeconds(1.0f); // Spell 애니메이션 대기

        animator.SetBool("isWalkAndAttack", true);

        hasCastSpell = true;
        isCasting = false;
    }

    private IEnumerator MoveAfterDelay()
    {
        

        yield return new WaitForSeconds(3.0f);


        // SpriteRenderer를 빨간색으로 변경
        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(1f, 0.5f, 0.5f, 1f);
        }

        yield return new WaitForSeconds(0.5f);

        // X 방향으로 3만큼 이동
        transform.position += new Vector3(0.01f, 0f, 0f);



        // 원래 색상으로 복구
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }

    }


}
