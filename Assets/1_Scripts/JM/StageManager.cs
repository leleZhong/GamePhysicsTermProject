using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance;

    [Header("Stage Settings")]
    int _score;
    public event Action<int> _onScoreChange;
    bool _isBossSpawned = false;    // ���� ���� üũ

    [Header("Enemy Settings")]  // �� ���� ����
    public Transform[] _enemy; // �� ������ �迭
    public Transform[] _myTF; // �� ���� ��ġ �迭
    public Transform _playerTF; // �÷��̾� Transform
    public float _delay; // �� ���� ������
    float _currentTime; // ���� �ð� Ȯ��

    [Header("Boss Settings")]
    public GameObject _boss;

    [Header("Target Object")]
    public GameObject targetObject; // ī�޶� �þ� Ȯ���� ��� ������Ʈ
    Camera _mainCamera;

    void Awake()
    {
        Instance = this;

        _score = 0;

        _mainCamera = Camera.main;
    }

    void Start()
    {
        _currentTime = Time.time + _delay + UnityEngine.Random.Range(2, 4f);
    }

    void Update()
    {
        if (_currentTime < Time.time && !_isBossSpawned)
        {
            Generate();
            _currentTime = Time.time + _delay + UnityEngine.Random.Range(0.5f, 2f);
        }

        if (SceneManager.GetActiveScene().name != "Stage2" && _score >= 50 && !_isBossSpawned)
        {
            // ���� ������ ����
            SpawnBoss();
        }

         // Ư�� ������Ʈ�� ī�޶� �þ߿� ���� �� ���� ���� ���·� ��ȯ
        if (targetObject != null && IsObjectInView(targetObject))
        {
            _isBossSpawned = true;
        }
    }

    void SpawnBoss()
    {
        _boss.SetActive(true);
        _isBossSpawned = true;

        Debug.Log("���� ����!");
    }

    void Generate()
    {
        Transform obj = _myTF[UnityEngine.Random.Range(0, _myTF.Length)];
        Vector3 dir = _playerTF.position - obj.position;
        Quaternion rotate = Quaternion.FromToRotation(Vector3.down, dir);

        int num = UnityEngine.Random.Range(0, _enemy.Length);
        Instantiate(_enemy[num], obj.position, rotate);
    }

    public void AddScore(int num)
    {
        _score += num;
        _onScoreChange?.Invoke(_score);
    }

    bool IsObjectInView(GameObject obj)
    {
        if (obj == null || _mainCamera == null) return false;

        // ������Ʈ�� ���� ��ǥ�� ����Ʈ ��ǥ�� ��ȯ
        Vector3 viewportPos = _mainCamera.WorldToViewportPoint(obj.transform.position);

        // ����Ʈ ���� �ִ��� Ȯ�� (x, y�� 0~1 ����, z > 0�̸� ī�޶� ��)
        return viewportPos.x > 0 && viewportPos.x < 1 &&
               viewportPos.y > 0 && viewportPos.y < 1 &&
               viewportPos.z > 0;
    }
}
