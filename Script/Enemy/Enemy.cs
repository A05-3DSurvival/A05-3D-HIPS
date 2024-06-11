using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static Enemy instance;

    public List<GameObject> dayEnemy;
    public GameObject nightEnemy;

    public DayNightCycle dayNight;

    public int monsterCount = 20;

    private bool isStart = false;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        if(isStart == false)
        {
            isStart = true;
        }
    }

    public void dayStart()
    {
        for (int i = 0; i < monsterCount; i++)
        {
            int randomIndex = Random.Range(0, dayEnemy.Count);
            float x = Random.Range(-100, 100);
            float z = Random.Range(-100, 100);
            Vector3 monsterPosition = new Vector3(x, 0, z);

            Instantiate(dayEnemy[randomIndex], monsterPosition, Quaternion.identity);
        }       
    }
}