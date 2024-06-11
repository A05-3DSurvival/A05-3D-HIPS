using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObject : MonoBehaviour
{
    public List<GameObject> gameObjectList;

    // Start is called before the first frame update
    void Start()
    {
        int randomIndex = Random.Range(0, gameObjectList.Count);

        GameObject go = Instantiate(gameObjectList[randomIndex], transform.position, Quaternion.identity);

        go.transform.parent = ResourceManager.Instance.parentTransform;

        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
