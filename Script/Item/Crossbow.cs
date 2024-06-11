using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crossbow : EquiupTool
{
    public GameObject arrowPrefab;
    public Transform arrowSpawnPoint;
    public ItemData arrowItem; // 화살 아이템 데이터
    public float arrowSpeed = 20f;

    private Camera crossBowCamera;
    public float useCrossbowStamina;

    void Start()
    {
        crossBowCamera = Camera.main;
        arrowItem = UIInventory.Instance.arrowItem; // 화살 아이템 데이터 참조
    }

    public override void OnAttackInput()
    {
        if (!attacking && UIInventory.Instance.HasItem(arrowItem, 1))
        {
            if (CharacterManager.Instance.Player.condition.UseStamina(useCrossbowStamina)) 
            {
                attacking = true;

                // 화살 발사 로직
                ShootArrow();

                // 공격 딜레이 지난 후 다시 공격 가능
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
        // 화살 개수 감소
        UIInventory.Instance.RemoveItem(arrowItem, 1);

        // 화살 생성 및 발사
        GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        rb.velocity = crossBowCamera.transform.forward * arrowSpeed;

        // 화살의 데미지 설정
        Arrow arrowScript = arrow.GetComponent<Arrow>();
        if (arrowScript != null)
        {
            arrowScript.damage = damage;
        }

        UIInventory.Instance.UpdateArrowCount();
    }
}
