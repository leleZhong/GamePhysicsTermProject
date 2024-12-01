using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyDamageHandler : MonoBehaviour
{
    [Header("Health Settings")]
    public int _hp;

    [Header("Visual Settings")]
    public Sprite[] _images; // 적 상태를 나타내는 스프라이트 배열
    public SpriteRenderer _spriteRenderer;

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
            if (SceneManager.GetActiveScene().name != "Stage2")
            {
                StageManager.Instance.AddScore(10);
                Destroy(gameObject);
            }
        }
    }
    
    void ReturnImage()
    {
        _spriteRenderer.sprite = _images[0];
    }

}
