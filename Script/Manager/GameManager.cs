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

        // MonsterPool 컴포넌트를 찾아서 ObjectPool에 할당
        ObjectPool = FindObjectOfType<MonsterPool>();
        if (ObjectPool == null)
        {
            Debug.LogError("MonsterPool 컴포넌트를 찾지 못했습니다.");
        }
    }
}
