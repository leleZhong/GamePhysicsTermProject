using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage3 : MonoBehaviour
{
    public static Stage3 Instance;

    [Header("Enemy Settings")]  // �� ���� ����
    public Transform[] _enemy; // �� ������ �迭
    public Transform[] _myTF; // �� ���� ��ġ �迭
    public Transform _playerTF; // �÷��̾� Transform
    public float _delay; // �� ���� ������
    float _currentTime; // ���� �ð� Ȯ��

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _currentTime = Time.time + _delay + UnityEngine.Random.Range(2, 4f);
    }

    void Update()
    {
        if (_currentTime < Time.time)
        {
            Generate();
            _currentTime = Time.time + _delay + UnityEngine.Random.Range(0.5f, 2f);
        }
    }

    void Generate()
    {
        Transform obj = _myTF[UnityEngine.Random.Range(0, _myTF.Length)];
        Vector3 dir = _playerTF.position - obj.position;
        Quaternion rotate = Quaternion.FromToRotation(Vector3.down, dir);

        int num = UnityEngine.Random.Range(0, _enemy.Length);
        Instantiate(_enemy[num], obj.position, rotate);
    }
}
