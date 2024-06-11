using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public ItemData itemToGive;

    public int quantityPerHit = 1;  // �ѹ� ä�� �� ������ �ڿ��� ��

    public int capacy;              // �� ��� ä���� �� �ִ���

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
