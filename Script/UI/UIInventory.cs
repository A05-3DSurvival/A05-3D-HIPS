using TMPro;
using UnityEngine;
using System.Collections;

public class UIInventory : MonoBehaviour
{
    public static UIInventory Instance { get; private set; }

    public ItemSlot[] slots;
    public GameObject inventoryWindow;
    public Transform slotPanel;
    public Transform dropPosition;

    [Header("���� �� ������")]
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedStatName;
    public TextMeshProUGUI selectedStatValue;

    public GameObject useBtn;
    public GameObject equiupBtn;
    public GameObject unEquiupBtn;
    public GameObject dropBtn;

    public PlayerController controller;
    public PlayerCondition condition;

    ItemData selectedItem;
    int selectedItemIndex;
    int curEquipIndex;

    public TextMeshProUGUI arrowCountText; // ȭ�� ������ ǥ���� TextMeshProUGUI
    public ItemData arrowItem; // ȭ�� ������ ������
    public ItemData woodItem; // ���� ������ ������

    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
        dropPosition = CharacterManager.Instance.Player.itemDropPoint;

        controller.inventory += Toggle;
        CharacterManager.Instance.Player.addItem += AddItem;

        inventoryWindow.SetActive(false);
        slots = new ItemSlot[slotPanel.childCount]; // �ڽ��� ������ ������ �� �ִ�.

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
        }
        ClearSelectedItemWindow();
        UpdateArrowCount();
    }

    void ClearSelectedItemWindow() // �ʱ�ȭ
    {
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        useBtn.SetActive(false);
        equiupBtn.SetActive(false);
        unEquiupBtn.SetActive(false);
        dropBtn.SetActive(false);
    }

    public void Toggle()
    {
        if (IsOPen())
        {
            inventoryWindow.SetActive(false);
        }
        else
        {
            inventoryWindow.SetActive(true);
        }
    }

    public bool IsOPen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.itemData;

        // �������� �ߺ� �������� üũ
        if (data.canStack)
        {
            ItemSlot slot = GetItemStack(data);

            if (slot != null)
            {
                slot.quantity++; // �ߺ����� ����
                UpdateUI();
                CharacterManager.Instance.Player.itemData = null;
                UpdateArrowCount();
                return;
            }
        }

        // �װ� �ƴ϶�� ����ִ� ������ �����´�.
        ItemSlot emptySlot = GetEmptySlot();
        // �ִٸ� �� ���Կ�
        if (emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.Player.itemData = null;
            UpdateArrowCount();
            return;
        }


        // ���ٸ� �������� ������.
        ThrowItem(data);
        CharacterManager.Instance.Player.itemData = null;
    }

    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                slots[i].Set();
            }

            else
            {
                slots[i].Clear();
            }
        }
    }

    void ThrowItem(ItemData data)
    {
        Instantiate(data.itemObject, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value));
    }

    ItemSlot GetItemStack(ItemData data)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == data && slots[i].quantity < data.maxStack)
            {
                return slots[i];
            }
        }
        return null;
    }



    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }
        return null;
    }

    public void SelectItem(int index)
    {
        if (slots[index].item == null) return;

        selectedItem = slots[index].item;
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.itemName;
        selectedItemDescription.text = selectedItem.itemDescription;

        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        for (int i = 0; i < selectedItem.consumables.Length; i++)
        {
            selectedStatName.text += selectedItem.consumables[i].type.ToString() + "\n";
            selectedStatValue.text += selectedItem.consumables[i].value.ToString() + "\n";
        }

        useBtn.SetActive(selectedItem.itemType == ItemType.Consumable);

        equiupBtn.SetActive(selectedItem.itemType == ItemType.Equipable && !slots[index].equipeed);
        unEquiupBtn.SetActive(selectedItem.itemType == ItemType.Equipable && slots[index].equipeed);

        dropBtn.SetActive(true);
    }


    public void OnUseButton()
    {
        if (selectedItem.itemType == ItemType.Consumable)
        {
            for (int i = 0; i < selectedItem.consumables.Length; i++)
            {
                switch (selectedItem.consumables[i].type)
                {
                    case ConsumableType.Health:
                        condition.Heal(selectedItem.consumables[i].value);
                        break;

                    case ConsumableType.Hunger:
                        condition.Eat(selectedItem.consumables[i].value);
                        break;
                }
            }
            RemoveSelectItem();
        }
    }
    public void OnEquipButton()
    {
        if (slots[curEquipIndex].equipeed)
        {
            // ��������
            UnEquip(curEquipIndex);
        }

        slots[selectedItemIndex].equipeed = true;
        curEquipIndex = selectedItemIndex;
        CharacterManager.Instance.Player.equipment.NewEquiup(selectedItem);
        UpdateUI();
        SelectItem(selectedItemIndex);
    }

    void UnEquip(int index)
    {
        slots[index].equipeed = false;
        CharacterManager.Instance.Player.equipment.UnEquiup();
        UpdateUI();

        if (selectedItemIndex == index)
        {
            SelectItem(selectedItemIndex);
        }
    }

    public void OnUnEquiupButton()
    {
        UnEquip(selectedItemIndex);
    }
    public void OnDropButton() // ������
    {
        ThrowItem(selectedItem);
        RemoveSelectItem();
    }

    void RemoveSelectItem() // ������ ���, �����⿡ ���� �κ��丮������ ���� ����
    {
        slots[selectedItemIndex].quantity--;
        if (slots[selectedItemIndex].quantity <= 0)
        {
            selectedItem = null;
            slots[selectedItemIndex].item = null;
            selectedItemIndex = -1;
            ClearSelectedItemWindow();
        }

        UpdateUI();
    }

    // ��� Ȯ�� �� �Ҹ� �޼��� �߰�
    public bool HasItem(ItemData item, int amount)
    {
        int totalAmount = 0;
        foreach (var slot in slots)
        {
            if (slot.item == item)
            {
                totalAmount += slot.quantity;
                if (totalAmount >= amount)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void RemoveItem(ItemData item, int amount)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == item)
            {
                if (slots[i].quantity >= amount)
                {
                    slots[i].quantity -= amount;
                    if (slots[i].quantity == 0)
                    {
                        slots[i].item = null;
                    }
                    UpdateUI();
                    UpdateArrowCount();
                    return;
                }
                else
                {
                    amount -= slots[i].quantity;
                    slots[i].item = null;
                    slots[i].quantity = 0;
                }
            }
        }
        UpdateUI();
        UpdateArrowCount();
    }

    public void AddCraftedItem(ItemData item, int quantity)
    {
        ItemSlot slot = GetItemStack(item);
        if (slot != null)
        {
            slot.quantity += quantity;
        }
        else
        {
            ItemSlot emptySlot = GetEmptySlot();
            if (emptySlot != null)
            {
                emptySlot.item = item;
                emptySlot.quantity = quantity;
            }
        }
        UpdateUI();
        UpdateArrowCount();
    }
    public void UpdateArrowCount()
    {
        if (arrowCountText != null)
        {
            int arrowCount = 0;
            foreach (var slot in slots)
            {
                if (slot.item == arrowItem)
                {
                    arrowCount += slot.quantity;
                }
            }
            arrowCountText.text = $"Arrows: {arrowCount}";
        }
    }

    public void RepairDoor(StructureObject door)
    {
        if (HasItem(woodItem, 1))
        {
            RemoveItem(woodItem, 3);
            door.Repair();
            RepairMessage.Instance.ShowRepairMessage("���� �� ���� �Ҹ��Ͽ� �ǹ��� �Ϻ��� �����߽��ϴ�.");
        }
        else
        {
            RepairMessage.Instance.ShowRepairMessage("������ ������ �����մϴ�.");
        }
    }
}
