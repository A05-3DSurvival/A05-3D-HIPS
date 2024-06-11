using System.Collections.Generic;
using UnityEngine;

public class BuildCheck : MonoBehaviour
{
    List<Collider> colliderList = new List<Collider>(); // �ݶ��̴��� ����, �浹�� ������Ʈ��

    [SerializeField] private LayerMask layerGrounded; // �����̾�, ���� �浹�� �����ϵ��� ����

    // ��ġ ������ �������� �ƴ����� �浹�� ���� üũ
    [SerializeField] private Material green;
    [SerializeField] private Material red;


    private void Update()
    {
        ChangeMaterial();
    }

    private void ChangeMaterial()
    {
        if (colliderList.Count > 0) // ����Ʈ�� �ϳ��� �ִٸ� = �浹�� ��ü�� �ϳ��� �ִٸ�
        {
            // ����
            SetColor(red);
        }
        else
        {
            // �ʷ�
            SetColor(green);
        }
    }

    private void SetColor(Material color)
    {
        // �θ� �ִ� ���
        var parentRenderer = this.GetComponent<Renderer>();
        if (parentRenderer != null)
        {
            SetObjectColor(parentRenderer, color);
        }

        // �ڽĵ� �ִ� ���
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
        Debug.Log("�浹�� �ݶ��̴��� ����: " + colliderList.Count);
        return colliderList.Count == 0;
    }
}
