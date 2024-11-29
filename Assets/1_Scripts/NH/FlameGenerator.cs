using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameGenerator : MonoBehaviour
{
    public GameObject flamePrefab1; // 플레임 프리팹
    public GameObject flamePrefab2;
    public GameObject flamePrefab3;

    public Transform flameSpawnPoint; // 플레임이 생성될 위치
    public float flameLifeTime = 2f; // 플레임 지속 시간
    public AudioSource audioSource;
    public AudioClip flameClip;

    public int bulletDamage = 10; // 기본 탄알 데미지

    // Animation Event에서 호출할 메서드
    public void SpawnFlame1()
    {
        GameObject flame = Instantiate(flamePrefab1, flameSpawnPoint.position, Quaternion.identity);
        GuidedBullet guidedBullet = flame.GetComponent<GuidedBullet>();
        audioSource.clip = flameClip;
        audioSource.loop = false;
        audioSource.Play();
        guidedBullet.damage = bulletDamage;
        Destroy(flame, flameLifeTime); // 일정 시간이 지나면 플레임 제거
    }

    public void SpawnFlame2()
    {
        GameObject flame = Instantiate(flamePrefab2, flameSpawnPoint.position, Quaternion.identity);
        GuidedBullet guidedBullet = flame.GetComponent<GuidedBullet>();
        audioSource.clip = flameClip;
        audioSource.loop = false;
        audioSource.Play();
        guidedBullet.damage = bulletDamage + 10;
        Destroy(flame, flameLifeTime); // 일정 시간이 지나면 플레임 제거
    }

    public void SpawnFlame3()
    {
        GameObject flame = Instantiate(flamePrefab3, flameSpawnPoint.position, Quaternion.identity);
        GuidedBullet guidedBullet = flame.GetComponent<GuidedBullet>();
        audioSource.clip = flameClip;
        audioSource.loop = false;
        audioSource.Play();
        guidedBullet.damage = bulletDamage + 20;
        Destroy(flame, flameLifeTime); // 일정 시간이 지나면 플레임 제거
    }
}
