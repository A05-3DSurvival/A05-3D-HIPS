using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crossbow : EquiupTool
{
    public GameObject arrowPrefab;
    public Transform arrowSpawnPoint;
    public ItemData arrowItem; // ȭ�� ������ ������
    public float arrowSpeed = 20f;

    private Camera crossBowCamera;
    public float useCrossbowStamina;

    void Start()
    {
        crossBowCamera = Camera.main;
        arrowItem = UIInventory.Instance.arrowItem; // ȭ�� ������ ������ ����
    }

    public override void OnAttackInput()
    {
        if (!attacking && UIInventory.Instance.HasItem(arrowItem, 1))
        {
            if (CharacterManager.Instance.Player.condition.UseStamina(useCrossbowStamina)) 
            {
                attacking = true;

                // ȭ�� �߻� ����
                ShootArrow();

                // ���� ������ ���� �� �ٽ� ���� ����
                Invoke("OnCanAttack", attackRate);
            }
        }
    }
    void OnCanAttack()
    {
        attacking = false;
    }

    void ShootArrow()
    {
        // ȭ�� ���� ����
        UIInventory.Instance.RemoveItem(arrowItem, 1);

        // ȭ�� ���� �� �߻�
        GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        rb.velocity = crossBowCamera.transform.forward * arrowSpeed;

        // ȭ���� ������ ����
        Arrow arrowScript = arrow.GetComponent<Arrow>();
        if (arrowScript != null)
        {
            arrowScript.damage = damage;
        }

        UIInventory.Instance.UpdateArrowCount();
    }
}
