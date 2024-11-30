using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character3Controller : MonoBehaviour
{
    public float maxHP = 100f;
    public Animator animator;
    public float currentHP;
    private bool isHurt = false;
    private bool isDead = false;
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
        AudioSource.Stop();
        AudioSource.clip = hurtSound;
        AudioSource.loop = false;
        AudioSource.Play();


        if (currentHP == maxHP / 2)
        {
            animator.SetBool("isAttack1", false);
            animator.SetBool("isAttack2", true);
        }

        if (currentHP == maxHP / 4)
        {
            animator.SetBool("isAttack2", false);
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
                Instantiate(DeathCloud, transform.position, Quaternion.identity);
                Destroy(gameObject, 0.5f);
            }

        }
    }

    // Look ���¿��� 3�� �� Attack1���� ��ȯ
    private IEnumerator StartAttack1AfterLook()
    {
        yield return new WaitForSeconds(lookTime);

       animator.SetBool("isAttack1", true); // Attack1 ���·� ��ȯ
    }
}
