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

    private float currentBlendValue = 0.5f;
    public float currentHP;
    private bool isCasting = false;
    private bool isHurt = false;
    private bool isDead = false;
    private bool isSpellNoEffect = false;
    public BackGround background;

    public GameObject assignedPlayer;

    void Start()
    {
        currentHP = maxHP;
        assignedPlayer = GameObject.FindGameObjectWithTag("Player");
        target = assignedPlayer.transform;
    }

    void Update()
    {
        if (isSpellNoEffect || isDead || isCasting || isHurt) return;

        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (distanceToTarget > attackRange)
        {
            currentBlendValue = Mathf.Lerp(currentBlendValue, 1f, Time.deltaTime * 5);
        }
        else
        {
            currentBlendValue = Mathf.Lerp(currentBlendValue, 0f, Time.deltaTime * 5);
            SetBackgroundSpeed(0);
        }

        animator.SetFloat("Blend", currentBlendValue);

        if (distanceToTarget > attackRange)
        {
            Vector3 direction = (target.position - transform.position).normalized;
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

        if (currentHP <= maxHP / 2 && !isCasting)
        {
            StartCoroutine(StartCast());
        }

        if (currentHP <= 0 && !isDead)
        {
            isDead = true;
            animator.SetTrigger("Death");
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

        isCasting = false;
    }
}
