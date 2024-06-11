using System.Collections.Generic;
using UnityEngine;

public class BuildCheck : MonoBehaviour
{
    List<Collider> colliderList = new List<Collider>(); // 콜라이더를 저장, 충돌한 오브젝트의

    [SerializeField] private LayerMask layerGrounded; // 지상레이어, 땅은 충돌을 무시하도록 설정

    // 설치 가능한 지역인지 아닌지를 충돌을 통해 체크
    [SerializeField] private Material green;
    [SerializeField] private Material red;


    private void Update()
    {
        ChangeMaterial();
    }

    private void ChangeMaterial()
    {
        if (colliderList.Count > 0) // 리스트에 하나라도 있다면 = 충돌한 물체가 하나라도 있다면
        {
            // 빨강
            SetColor(red);
        }
        else
        {
            // 초록
            SetColor(green);
        }
    }

    private void SetColor(Material color)
    {
        // 부모만 있는 경우
        var parentRenderer = this.GetComponent<Renderer>();
        if (parentRenderer != null)
        {
            SetObjectColor(parentRenderer, color);
        }

        // 자식도 있는 경우
        foreach (Transform tfChild in this.transform)
        {
            var childRenderer = tfChild.GetComponent<Renderer>();
            if (childRenderer != null)
            {
                SetObjectColor(childRenderer, color);
            }
        }
    }

    private void SetObjectColor(Renderer renderer, Material color)
    {
        var newMaterial = new Material[renderer.materials.Length];
        for (int i = 0; i < renderer.materials.Length; i++)
        {
            newMaterial[i] = color;
        }
        renderer.materials = newMaterial;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != layerGrounded)
        {
            colliderList.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != layerGrounded)
        {
            colliderList.Remove(other);
        }
    }

    public bool IsAbleBuild()
    {
        Debug.Log("충돌된 콜라이더의 갯수: " + colliderList.Count);
        return colliderList.Count == 0;
    }
}
