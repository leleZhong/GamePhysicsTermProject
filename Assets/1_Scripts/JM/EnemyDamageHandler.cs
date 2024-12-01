using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyDamageHandler : MonoBehaviour
{
    [Header("Health Settings")]
    public float _maxHP;
    [SerializeField]
    float _hp;

    [Header("Visual Settings")]
    public Sprite[] _images; // �� ���¸� ��Ÿ���� ��������Ʈ �迭
    public SpriteRenderer _spriteRenderer;
    public Animator _anim;

    [Header("Boss Settings")]
    public Boss3Controller _bossController; // ���� ��Ʈ�ѷ� ����

    void Awake()
    {
        _hp = _maxHP;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "bullet" :
                Bullet bullet = other.GetComponent<Bullet>();

                OnHit(bullet._pow);
                PoolManager.Despawn(other.gameObject);
                break;
            
            case "Player" :
                other.gameObject.SetActive(false);
                break;
        }
    }

    public void OnHit(int dmg)
    {
        _hp -= dmg;
        if (_images.Length > 0)
        {
            _spriteRenderer.sprite = _images[1];
            Invoke("ReturnImage", 0.1f);
        }
        if (_hp <= 0)
        {
            StageManager.Instance.AddScore(10);
            Destroy(gameObject);
        }
        if (gameObject.tag == "Boss" && SceneManager.GetActiveScene().name == "Stage3")
        {
            _anim.SetTrigger("OnHit");
        }
    }
    
    void ReturnImage()
    {
        _spriteRenderer.sprite = _images[0];
    }

    public float GetCurrentHealth()
    {
        return _hp;
    }

}
