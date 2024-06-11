using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerCondition condition;
    public Equiupment equipment;

    public ItemData itemData;
    public StructureData structureData;
    public Action addItem;

    public Transform itemDropPoint;

    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
        equipment = GetComponent<Equiupment>();
    }
}
