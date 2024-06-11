using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[System.Serializable]
public class Craft
{
    public string craftName;
    public GameObject go_prefab; // 실제 설치 프리펩
    public GameObject go_PreviewPrefab; // 설치 미리보기 프리펩
    public ItemData[] requiredMaterials; // 필요한 재료 목록
    public int[] requiredAmounts; // 각 재료의 수량
}

public class CraftManual : MonoBehaviour
{
    // 상태변수
    public bool isActivated = false;
    private bool isPreviewActivated = false;

    public GameObject go_BaseUI;

    [SerializeField] private Craft[] craft; //  건축물 저장 배열,

    private GameObject go_Preview; // 미리보기 프리펩
    private GameObject go_BuildPrefab; // 실제 생성오브젝트

    [SerializeField] private Transform playerTf; //플레이어 위치

    private RaycastHit hitInfo;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float rayDistance;

    [Header("오류 메세지")]
    [SerializeField] private GameObject errorLog;

    private void Start()
    {
        go_BaseUI.SetActive(false);
        errorLog.SetActive(false);
    }

    public void SlotClick(int slotNum) // 버튼 클릭 함수
    {
        Craft selectedCraft = craft[slotNum];
        Debug.Log(selectedCraft);
        // 필요한 재료가 충분한지 확인
        if (!HasRequiredMaterials(selectedCraft))
        {
            // 재료 부족 메시지 표시
            StartCoroutine(DisableErrorLogAfterDelay(1f)); // 3초 후에 오류 로그 비활성화
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
                // 재료 부족 메시지 표시
                Debug.Log("재료가 부족합니다.");
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
