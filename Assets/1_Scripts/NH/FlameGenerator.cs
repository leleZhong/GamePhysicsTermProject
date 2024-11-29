using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameGenerator : MonoBehaviour
{
    public GameObject flamePrefab1; // �÷��� ������
    public GameObject flamePrefab2;
    public GameObject flamePrefab3;

    public Transform flameSpawnPoint; // �÷����� ������ ��ġ
    public float flameLifeTime = 2f; // �÷��� ���� �ð�
    public AudioSource audioSource;
    public AudioClip flameClip;

    public int bulletDamage = 10; // �⺻ ź�� ������

    // Animation Event���� ȣ���� �޼���
    public void SpawnFlame1()
    {
        GameObject flame = Instantiate(flamePrefab1, flameSpawnPoint.position, Quaternion.identity);
        GuidedBullet guidedBullet = flame.GetComponent<GuidedBullet>();
        audioSource.clip = flameClip;
        audioSource.loop = false;
        audioSource.Play();
        guidedBullet.damage = bulletDamage;
        Destroy(flame, flameLifeTime); // ���� �ð��� ������ �÷��� ����
    }

    public void SpawnFlame2()
    {
        GameObject flame = Instantiate(flamePrefab2, flameSpawnPoint.position, Quaternion.identity);
        GuidedBullet guidedBullet = flame.GetComponent<GuidedBullet>();
        audioSource.clip = flameClip;
        audioSource.loop = false;
        audioSource.Play();
        guidedBullet.damage = bulletDamage + 10;
        Destroy(flame, flameLifeTime); // ���� �ð��� ������ �÷��� ����
    }

    public void SpawnFlame3()
    {
        GameObject flame = Instantiate(flamePrefab3, flameSpawnPoint.position, Quaternion.identity);
        GuidedBullet guidedBullet = flame.GetComponent<GuidedBullet>();
        audioSource.clip = flameClip;
        audioSource.loop = false;
        audioSource.Play();
        guidedBullet.damage = bulletDamage + 20;
        Destroy(flame, flameLifeTime); // ���� �ð��� ������ �÷��� ����
    }
}
