using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    static GameObject _folder;
    static Dictionary<int, List<GameObject>> _dic = new Dictionary<int, List<GameObject>>();

    public static GameObject Spawn(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        if (_folder == null)
            _folder = new GameObject("Pool");

        if (_dic.ContainsKey(prefab.GetInstanceID()))
        {
            List<GameObject> list = _dic[prefab.GetInstanceID()];

            for (int i=0; i < list.Count; i++)
            {
                if (list[i].activeSelf == false)
                {
                    GameObject clone = list[i];
                    clone.transform.position = pos;
                    clone.transform.rotation = rot;
                    clone.SetActive(true);
                    return clone;
                }
            }
        }
        return Create(prefab, pos, rot);
    }

    static GameObject Create(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        GameObject newClone = Instantiate(prefab, pos, rot);
        if (_dic.ContainsKey(prefab.GetInstanceID()))
            _dic[prefab.GetInstanceID()].Add(newClone);
        else
        {
            List<GameObject> list = new List<GameObject>
            {
                newClone
            };
            _dic.Add(prefab.GetInstanceID(), list);
        }
        return newClone;
    }

    public static void Despawn(GameObject obj)
    {
        obj.SetActive(false);
    }
}
