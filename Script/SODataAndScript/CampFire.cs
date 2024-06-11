using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampFire : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerCondition>(out PlayerCondition condition))
        {
            CharacterManager.Instance.Player.condition.TempFire(4);
        }
    }
}
