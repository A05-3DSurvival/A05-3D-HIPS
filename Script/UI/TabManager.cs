using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabManager : MonoBehaviour
{
    [SerializeField] private GameObject buildingUI;
    [SerializeField] private GameObject craftingUI;

    public void ShowBuildingUI()
    {
        buildingUI.SetActive(true);
        craftingUI.SetActive(false);
    }

    public void ShowCraftingUI()
    {
        buildingUI.SetActive(false);
        craftingUI.SetActive(true);
    }
}
