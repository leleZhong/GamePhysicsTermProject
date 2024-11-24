using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollider : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "bullet":  
                PoolManager.Despawn(other.gameObject);
                break;
            case "enemy":
            case "enemyBullet":
                Destroy(other.gameObject);
                break;
        }
    }
}
