using UnityEngine;
using System;

public class PlayerCondition : MonoBehaviour, IDamageable
{
    public UICondition uiCondition;

    Condition health { get { return uiCondition.health; } }
    Condition hunger {  get { return uiCondition.hunger; } }
    Condition stamina {  get { return uiCondition.stamina; } }
    Condition temp {  get { return uiCondition.temp; } }

    public event Action onTakeDamage; // Ãß°¡
    [SerializeField] private float noHungerHealthDecay;
    [SerializeField] private GameObject deadObj;

    [SerializeField] private GameObject sunCheck;

    private void Start()
    {
        Time.timeScale = 1f;
        deadObj.SetActive(false);
    }

    private void Update()
    {
        hunger.Subtract(hunger.passiveValue * Time.deltaTime);

        if (hunger.curValue == 0f)
        {
            health.Subtract(noHungerHealthDecay * Time.deltaTime);
            stamina.Add(stamina.passiveValue / 3 * Time.deltaTime);
        }
        else
        {
            stamina.Add(stamina.passiveValue * Time.deltaTime);
        }

        if (health.curValue == 0f)
        {
            Die();
            deadObj.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }

        if (!sunCheck.activeInHierarchy)
        {
            temp.Subtract(temp.passiveValue * Time.deltaTime);
        }

        if (temp.curValue == 0f)
        {
            health.Subtract(noHungerHealthDecay * Time.deltaTime);
        }
    }

    private void Die()
    {
        Time.timeScale = 0f;
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void Eat(float amount)
    {
        hunger.Add(amount);
    }

    public void TakePhysicalDamage(int damage)
    {
        health.Subtract(damage);
        onTakeDamage?.Invoke();
    }

    public bool UseStamina(float amount)
    {
        if (stamina.curValue - amount < 0f)
        {
            return false;
        }

        stamina.Subtract(amount);
        return true;
    }

    public void TempFire(int amount)
    {
        temp.Add(amount);
    }
}
