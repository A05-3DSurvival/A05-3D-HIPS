using System;
using UnityEngine;

public enum ItemType
{
    Equipable = 0,
    Consumable = 1,
    Resource = 2,
}

public enum ConsumableType
{
    Health = 0,
    Hunger = 1,
}


[Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public float value;
}
public enum ResourceType
{
    None,
    Wood,
    Rock,
    WoodStic
}

[CreateAssetMenu(fileName ="Item", menuName = "new Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string itemName;
    public string itemDescription;
    public ItemType itemType;
    public Sprite itemIcon;
    public GameObject itemObject;

    [Header("Stacking")]
    public bool canStack;
    public int maxStack;

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;

    [Header("장착")]
    public GameObject equiupPrefab;

    [Header("소모 재료")]
    public ResourceType resourceType;
}
