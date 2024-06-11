using UnityEngine;
using UnityEngine.InputSystem;

public class Equiupment : MonoBehaviour
{
    public Equiup curEquiup;
    public Transform equiupParent;

    private PlayerController controller;
    private PlayerCondition condition;

    private void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
    }

    public void NewEquiup(ItemData data)  // �ŰԺ����� ������ ����Ÿ ����ؾ���
    {
        UnEquiup();
        curEquiup = Instantiate(data.equiupPrefab, equiupParent).GetComponent<Equiup>();
    }

    public void UnEquiup()
    {
        if (curEquiup != null)
        {
            Destroy(curEquiup.gameObject);
            curEquiup = null;
        }
    }

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        // ��ư Ŭ�� �� ���� ��ǲ�� ����Ǿ� ���� 
        if (context.phase == InputActionPhase.Started && curEquiup != null && controller.canLook)
        {
            curEquiup.OnAttackInput();
        }
    }
}
