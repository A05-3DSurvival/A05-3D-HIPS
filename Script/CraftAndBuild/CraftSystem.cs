using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class Recipe
{
    public string recipeName;
    public Ingredient[] materials;
    public ItemData resultItem;
    public int resultAmount;
}

[Serializable]
public class Ingredient
{
    public ItemData itemData;
    public int quantity;
}

public class CraftSystem : MonoBehaviour
{
    public Recipe[] recipes;
    [SerializeField] private GameObject craftBaseUI;
    private CraftManual craftManual;
    [SerializeField] private GameObject errorLog;

    void Start()
    {
        craftManual = GetComponent<CraftManual>();
        craftBaseUI.SetActive(false);
    }
    private void Update()
    {
        if (craftBaseUI.activeInHierarchy && Keyboard.current.bKey.wasPressedThisFrame)
        {
            Off();
        }
    }
    public void SlotClick(int slotNum)
    {
        if (slotNum < 0 || slotNum >= recipes.Length) return;

        Recipe selectedRecipe = recipes[slotNum];

        // 필요한 재료가 충분한지 확인
        if (!HasRequiredMaterials(selectedRecipe))
        {
            // 재료 부족 메시지 표시
            StartCoroutine(DisableErrorLogAfterDelay(1f)); // 1초 후에 오류 로그 비활성화
            Off();
            return;
        }

        CraftItem(selectedRecipe);
        Off();
    }

    private bool HasRequiredMaterials(Recipe recipe)
    {
        foreach (var ingredient in recipe.materials)
        {
            if (!UIInventory.Instance.HasItem(ingredient.itemData, ingredient.quantity))
            {
                return false;
            }
        }
        return true;
    }

    private IEnumerator DisableErrorLogAfterDelay(float delay)
    {
        // 재료 부족 메시지 표시
        errorLog.SetActive(true);
        yield return new WaitForSeconds(delay);
        errorLog.SetActive(false);
    }

    void Off()
    {
        craftBaseUI.SetActive(false);
        craftManual.isActivated = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void CraftItem(Recipe recipe)
    {
        foreach (var ingredient in recipe.materials)
        {
            UIInventory.Instance.RemoveItem(ingredient.itemData, ingredient.quantity);
        }
        UIInventory.Instance.AddCraftedItem(recipe.resultItem, recipe.resultAmount); // 제작된 아이템 추가
    }
}
