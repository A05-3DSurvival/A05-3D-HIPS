using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public ItemData itemToGive;

    public int quantityPerHit = 1;  // 한번 채취 시 나오는 자원의 량

    public int capacy;              // 총 몇번 채취할 수 있는지

    public void Gather(Vector3 hitPoint, Vector3 hitNormal)
    {
        for ( int i = 0; i < quantityPerHit; i++ )
        {
            if ( capacy <= 0 )
            {
                break;
            }

            capacy--;

            GameObject go = Instantiate(itemToGive.itemObject, hitPoint, Quaternion.LookRotation(hitNormal, Vector3.up));

            go.transform.parent = ResourceManager.Instance.parentTransform;
        }
    }
}
