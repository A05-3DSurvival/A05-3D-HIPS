using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public MonsterPool ObjectPool { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // MonsterPool ������Ʈ�� ã�Ƽ� ObjectPool�� �Ҵ�
        ObjectPool = FindObjectOfType<MonsterPool>();
        if (ObjectPool == null)
        {
            Debug.LogError("MonsterPool ������Ʈ�� ã�� ���߽��ϴ�.");
        }
    }
}
