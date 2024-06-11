using UnityEngine;
using System.Collections.Generic;

public class PrefabManager : MonoBehaviour
{
    private static PrefabManager _instance;
    public static PrefabManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PrefabManager>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("PrefabManager");
                    _instance = obj.AddComponent<PrefabManager>();
                }
            }
            return _instance;
        }
    }

    private Dictionary<GameObject, GameObject> prefabLookup = new Dictionary<GameObject, GameObject>();

    public GameObject InstantiatePrefab(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        GameObject instance = Instantiate(prefab, position, rotation);
        if (!prefabLookup.ContainsKey(instance))
        {
            prefabLookup.Add(instance, prefab);
        }
        return instance;
    }

    public GameObject GetPrefab(GameObject instance)
    {
        if (prefabLookup.TryGetValue(instance, out GameObject prefab))
        {
            return prefab;
        }
        return null;
    }
}
