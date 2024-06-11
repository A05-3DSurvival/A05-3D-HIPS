using UnityEngine;

public class StructureObject : MonoBehaviour, IInteractable, IDamageable
{
    public StructureData data;
    public Animator doorAnimator;
    private bool isDoorOpen = false;
    public int health = 100;
    public int maxHealth = 100; // �ִ� ������
    public GameObject repairText;

    [SerializeField] GameObject house;

    public string GetInteractionDescription()
    {
        string description = $"{data.StructName}\n{data.StructDescription} ���� ������ : {health} / {maxHealth}";
        return description;
    }

    public void OnInteract()
    {
        if (TryGetComponent<Animator>(out Animator doorAnimator))
        {
            isDoorOpen = !isDoorOpen;
            doorAnimator.SetBool("Open", isDoorOpen);
        }
    }

    public void TakePhysicalDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(house);
        }
    }

    // ���� �޼��� �߰�
    public void Repair()
    {
        health = maxHealth;
    }
}
