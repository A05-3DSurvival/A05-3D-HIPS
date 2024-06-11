using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquiupTool : Equiup
{
    public float attackRate;
    protected bool attacking; // �ִϸ��̼� Ʈ���� �� ��
    public float attackDistance;

    private Animator animator;
    private Camera cam;
    public float useStamina;


    [Header("�ڿ� ü�� ���� ����")]
    public bool doesGetherResources;

    [Header("���� ���� ���� ����")]
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
