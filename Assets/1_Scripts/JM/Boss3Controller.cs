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
    public Transform _bullet1; // 패턴 1 총알
    public Transform _bullet2; // 패턴 2 총알
    float _currentTime; // 발사 간격 타이머
    public float pattern1Interval = 0.5f; // 패턴 1의 연속 발사 간격
    int pattern1Shots = 4; // 패턴 1에서 발사할 총알 개수
    bool isPattern2Active = false; // 패턴 2 활성화 여부
    
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
            yield return new WaitForSeconds(pattern1Interval); // 총알 간격
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
            int bulletCount = 8; // 방사형 총알 개수
            float angleStep = 360f / bulletCount;

            for (int i = 0; i < bulletCount; i++)
            {
                float angle = i * angleStep;
                Quaternion rotation = Quaternion.Euler(0, 0, angle);

                // 총알 생성 및 방향 설정
                Transform bullet = Instantiate(_bullet2, _myTF.position, rotation);
                Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
                if (bulletRb != null)
                {
                    Vector2 direction = rotation * Vector2.up; // 회전 값을 기준으로 방향 계산
                    bulletRb.velocity = direction * 10f; // 속도 설정
                }
            }
        }

        // 패턴 2가 발동한 뒤 다시 활성화 가능하도록 상태 초기화
        StartCoroutine(ResetPattern2());
    }

    IEnumerator ResetPattern2()
    {
        yield return new WaitForSeconds(5f); // 패턴 2의 재발동 대기 시간
        isPattern2Active = false;
    }
}
