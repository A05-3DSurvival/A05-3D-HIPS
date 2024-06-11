using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[System.Serializable]
public class Craft
{
    public string craftName;
    public GameObject go_prefab; // ���� ��ġ ������
    public GameObject go_PreviewPrefab; // ��ġ �̸����� ������
    public ItemData[] requiredMaterials; // �ʿ��� ��� ���
    public int[] requiredAmounts; // �� ����� ����
}

public class CraftManual : MonoBehaviour
{
    // ���º���
    public bool isActivated = false;
    private bool isPreviewActivated = false;

    public GameObject go_BaseUI;

    [SerializeField] private Craft[] craft; //  ���๰ ���� �迭,

    private GameObject go_Preview; // �̸����� ������
    private GameObject go_BuildPrefab; // ���� ����������Ʈ

    [SerializeField] private Transform playerTf; //�÷��̾� ��ġ

    private RaycastHit hitInfo;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float rayDistance;

    [Header("���� �޼���")]
    [SerializeField] private GameObject errorLog;

    private void Start()
    {
        go_BaseUI.SetActive(false);
        errorLog.SetActive(false);
    }

    public void SlotClick(int slotNum) // ��ư Ŭ�� �Լ�
    {
        Craft selectedCraft = craft[slotNum];
        Debug.Log(selectedCraft);
        // �ʿ��� ��ᰡ ������� Ȯ��
        if (!HasRequiredMaterials(selectedCraft))
        {
            // ��� ���� �޽��� ǥ��
            StartCoroutine(DisableErrorLogAfterDelay(1f)); // 3�� �Ŀ� ���� �α� ��Ȱ��ȭ
            return;
        }

        if (selectedCraft.go_PreviewPrefab != null)
        {
            go_Preview = PrefabManager.Instance.InstantiatePrefab(selectedCraft.go_PreviewPrefab, playerTf.position + playerTf.forward, Quaternion.identity);
            go_BuildPrefab = selectedCraft.go_prefab;
            isPreviewActivated = true;
            Close();
        }
    }

    void Update()
    {
        if (isPreviewActivated)
        {
            PreviewPositionUpdate();
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            Build();
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cancel();
        }
    }

    private void Build()
    {
        if (isPreviewActivated && go_Preview.GetComponent<BuildCheck>().IsAbleBuild())
        {
            Craft selectedCraft = GetSelectedCraft();
            if (selectedCraft != null && HasRequiredMaterials(selectedCraft))
            {
                UseMaterials(selectedCraft);
                PrefabManager.Instance.InstantiatePrefab(go_BuildPrefab, hitInfo.point, Quaternion.identity);

                Destroy(go_Preview);
                isActivated = false;
                isPreviewActivated = false;
                go_Preview = null;
                go_BuildPrefab = null;
            }
            else
            {
                // ��� ���� �޽��� ǥ��
                Debug.Log("��ᰡ �����մϴ�.");
            }
        }
    }

    private Craft GetSelectedCraft()
    {
        foreach (var craftItem in craft)
        {
            if (PrefabManager.Instance.GetPrefab(go_Preview) == craftItem.go_PreviewPrefab)
            {
                return craftItem;
            }
        }
        return null;
    }

    private bool HasRequiredMaterials(Craft craft)
    {
        for (int i = 0; i < craft.requiredMaterials.Length; i++)
        {
            if (!UIInventory.Instance.HasItem(craft.requiredMaterials[i], craft.requiredAmounts[i]))
            {
                return false;
            }
        }
        return true;
    }

    private void UseMaterials(Craft craft)
    {
        for (int i = 0; i < craft.requiredMaterials.Length; i++)
        {
            UIInventory.Instance.RemoveItem(craft.requiredMaterials[i], craft.requiredAmounts[i]);
        }
    }

    private void PreviewPositionUpdate()
    {
        if (Physics.Raycast(playerTf.position, playerTf.forward, out hitInfo, rayDistance, layerMask))
        {
            if (hitInfo.transform != null)
            {
                Vector3 location = hitInfo.point;
                go_Preview.transform.position = location;
            }
        }
    }

    void Cancel()
    {
        if (isPreviewActivated)
            Destroy(go_Preview);

        isPreviewActivated = false;
        go_Preview = null;
        go_BuildPrefab = null;
        Close();
    }

    public void OnBuild(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (!isActivated && !isPreviewActivated)
            {
                Open();
            }
            else
            {
                Close();
            }
        }
    }

    private void Open()
    {
        isActivated = true;
        go_BaseUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void Close()
    {
        isActivated = false;
        go_BaseUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    private IEnumerator DisableErrorLogAfterDelay(float delay)
    {
        errorLog.SetActive(true);
        yield return new WaitForSeconds(delay);
        errorLog.SetActive(false);
    }
}
