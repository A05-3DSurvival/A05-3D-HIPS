using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Strucuture", menuName = "new Structure")]

public class StructureData : ScriptableObject
{
    [Header("Info")]
    public string StructName;
    public string StructDescription;
}
