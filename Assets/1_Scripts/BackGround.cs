using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    Transform _myTF;
    public float _speed;

    void Start()
    {
        _myTF = GetComponent<Transform>();
    }

    void Update()
    {
        _myTF.Translate(Vector2.left * _speed * Time.deltaTime);
        if (_myTF.position.x <= -12)
        {
            _myTF.Translate(Vector2.right * 12);
        }
    }
}
