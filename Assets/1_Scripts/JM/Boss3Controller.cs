using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3Controller : MonoBehaviour
{

    public Animator _animator;

    [Header("References")]
    public Transform _myTF;
    public EnemyDamageHandler _damageHandler;

    [Header("Attack Settings")]
    public Transform _bullet1; // ���� 1 �Ѿ�
    public Transform _bullet2; // ���� 2 �Ѿ�
    float _currentTime; // �߻� ���� Ÿ�̸�
    public float pattern1Interval = 0.5f; // ���� 1�� ���� �߻� ����
    int pattern1Shots = 4; // ���� 1���� �߻��� �Ѿ� ����
    bool isPattern2Active = false; // ���� 2 Ȱ��ȭ ����
    
    void Update()
    {
        if (_damageHandler.GetCurrentHealth() <= _damageHandler._maxHP * 0.8f && !isPattern2Active)
        {
            Pattern2();
            isPattern2Active = true;
        }

        if (_currentTime < Time.time)
        {
            StartCoroutine(Pattern1());
            _currentTime = Time.time + Random.Range(4f, 6f);
        }
    }

    IEnumerator Pattern1()
    {
        for (int i = 0; i < pattern1Shots; i++)
        {
            _animator.SetBool("Pattern1", true);
            FireBullet1();
            yield return new WaitForSeconds(pattern1Interval); // �Ѿ� ����
            _animator.SetBool("Pattern1", false);
        }
    }

    void FireBullet1()
    {
        Vector3 dir = StageManager.Instance._playerTF.position - _myTF.position;
        Quaternion rotate = Quaternion.FromToRotation(Vector3.up, dir);

        if (_bullet1 != null)
        {
            Instantiate(_bullet1, _myTF.position, rotate);
        }
    }

    void Pattern2()
    {
        if (_bullet2 != null)
        {
            int bulletCount = 8; // ����� �Ѿ� ����
            float angleStep = 360f / bulletCount;

            for (int i = 0; i < bulletCount; i++)
            {
                float angle = i * angleStep;
                Quaternion rotation = Quaternion.Euler(0, 0, angle);

                // �Ѿ� ���� �� ���� ����
                Transform bullet = Instantiate(_bullet2, _myTF.position, rotation);
                Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
                if (bulletRb != null)
                {
                    Vector2 direction = rotation * Vector2.up; // ȸ�� ���� �������� ���� ���
                    bulletRb.velocity = direction * 10f; // �ӵ� ����
                }
            }
        }

        // ���� 2�� �ߵ��� �� �ٽ� Ȱ��ȭ �����ϵ��� ���� �ʱ�ȭ
        StartCoroutine(ResetPattern2());
    }

    IEnumerator ResetPattern2()
    {
        yield return new WaitForSeconds(5f); // ���� 2�� ��ߵ� ��� �ð�
        isPattern2Active = false;
    }
}
