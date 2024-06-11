using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquiupTool : Equiup
{
    public float attackRate;
    protected bool attacking; // 애니메이션 트리거 불 값
    public float attackDistance;

    private Animator animator;
    private Camera cam;
    public float useStamina;


    [Header("자원 체취 가능 여부")]
    public bool doesGetherResources;

    [Header("전투 무기 구분 여부")]
    public bool doesCombatWapone;
    public int damage;
    void Start()
    {
        animator = GetComponent<Animator>();
        cam = Camera.main;
    }

    public override void OnAttackInput()
    {
        if (!attacking)
        {
            if (CharacterManager.Instance.Player.condition.UseStamina(useStamina))
            {
                attacking = true;
                animator.SetTrigger("Attack");

                Invoke("OnCanAttack", attackRate);
            }
        }
    }

    void OnCanAttack()
    {
        attacking = false;
    }

    void OnHit()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, attackDistance))
        {
            if (doesGetherResources && hit.collider.TryGetComponent(out Resource resource))
            {
                resource.Gather(hit.point, hit.normal);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (attacking)
        {
            if (other.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                damageable.TakePhysicalDamage(damage);
            }
        }
    }
}
