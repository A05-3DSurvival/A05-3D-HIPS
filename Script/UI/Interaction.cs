using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f;
    public float lastChectTime;
    public float maxCheckDsitance;
    public LayerMask layerMask;

    public GameObject curInteractGameObject;
    public IInteractable curInteractable; // ���� �������̽� �߰��� ����

    public TextMeshProUGUI promptText; // ��ȣ�ۿ� �ؽ�Ʈ
    private Camera camera;

    private void Start()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        if (Time.time - lastChectTime > checkRate)
        {
            lastChectTime = Time.time;

            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxCheckDsitance, layerMask)) 
            {
                if (hit.collider.gameObject != curInteractGameObject)
                {
                    curInteractGameObject = hit.collider.gameObject;
                    curInteractable = hit.collider.gameObject.GetComponent<IInteractable>();

                    SetPromptText();
                }
            }
        }
    }

    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true);
        promptText.text = curInteractable.GetInteractionDescription();
    }

    public void OnInteractInput(InputAction.CallbackContext context) // �÷��̾��� Interaction�׼� ����
    {
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }
}