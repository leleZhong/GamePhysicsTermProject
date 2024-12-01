using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character3Controller : MonoBehaviour
{
    public float maxHP = 100f;
    public Animator animator;
    public float currentHP;
    private bool isHurt = false;
    public bool isDead { get; private set; } = false;
    public GameObject assignedPlayer;
    public GameObject DeathCloud;

    private bool hasStartedAttack1 = false; // Attack1 ���� ����
    private float lookTime = 3f; // Look ���� ���� �ð� Ÿ�̸�

    public AudioSource AudioSource;
    public AudioClip hurtSound;
    public AudioClip DieSound;


    void Start()
    {
        currentHP = maxHP;
        assignedPlayer = GameObject.FindGameObjectWithTag("Player");

        // Look ���� Ÿ�̸� �ڷ�ƾ ����
        StartCoroutine(StartAttack1AfterLook());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("bullet") && !isHurt)
        {
            TakeDamage(10);
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead || isHurt) return;

        currentHP -= damage;
        isHurt = true;
        animator.SetBool("isHurt", true);

        // ���� ��ȯ�� �����ϱ� ���� �ٸ� �Ķ���� ����
        animator.SetBool("isAttack1", false);
        animator.SetBool("isAttack2", false);
        animator.SetBool("isAttack3", false);

        AudioSource.Stop();
        AudioSource.clip = hurtSound;
        AudioSource.loop = false;
        AudioSource.Play();

        StartCoroutine(HurtCooldown());

        if (currentHP <= maxHP / 2)
        {
            animator.SetBool("isAttack2", true);
        }

        if (currentHP <= maxHP / 4)
        {
            animator.SetBool("isAttack3", true);
        }

        if (currentHP <= 0 && !isDead)
        {
            isDead = true;
            animator.SetTrigger("Death");
            if (isDead == true)
            {
                AudioSource.Stop();
                AudioSource.clip = DieSound;
                AudioSource.loop = true;
                AudioSource.Play();
                GameObject deathcloud = Instantiate(DeathCloud, transform.position, Quaternion.identity);
                Destroy(GameObject.Find("AppearBoss"));
                Destroy(deathcloud, 3f);
                Destroy(gameObject, 1f);
            }
        }
    }

    // Look ���¿��� 3�� �� Attack1���� ��ȯ
    private IEnumerator StartAttack1AfterLook()
    {
        yield return new WaitForSeconds(lookTime);

       animator.SetBool("isAttack1", true); // Attack1 ���·� ��ȯ
    }

    private IEnumerator HurtCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        isHurt = false;
        animator.SetBool("isHurt", false);
    }
}

