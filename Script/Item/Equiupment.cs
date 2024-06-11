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

    public void NewEquiup(ItemData data)  // 매게변수로 아이템 데이타 사용해야함
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
        // 버튼 클릭 시 어택 인풋이 실행되어 조건 
        if (context.phase == InputActionPhase.Started && curEquiup != null && controller.canLook)
        {
            curEquiup.OnAttackInput();
        }
    }
}
