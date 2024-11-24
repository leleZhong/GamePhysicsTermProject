using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float _speed;
    public Rigidbody2D _rd;
    public Transform _myTF;
    public int _pow;

    void OnEnable()
    {
        _rd.velocity = Vector2.zero;
        _rd.AddForce(_myTF.up * _speed, ForceMode2D.Impulse);
    }
}
