using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayEnemy : MonoBehaviour, IDamageable
{
    public GameObject nightEnemy;

    

    [Header("Stats")]
    public int health;

    private void Update()
    {
        if(Enemy.instance.dayNight.time >= 0.75)
        {
            Evolution();
        }

    }

    public void TakePhysicalDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    public void Evolution()
    {
       GameObject enemy = GameManager.Instance.ObjectPool.SpawnFromPool("Enemy");
        if (enemy != null)
        {
            enemy.GetComponent<NightEnemy>().agent.enabled = true;
            enemy.transform.position = transform.position;
            enemy.transform.rotation = Quaternion.identity;
        }

        Destroy(gameObject);
    }

}
