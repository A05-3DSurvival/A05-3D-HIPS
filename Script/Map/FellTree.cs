using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.Port;

public class FellTree : MonoBehaviour
{
    public float capacyTimer;

    public float growthTime = 5;    // 성장하는데 걸리는 시간

    public GameObject Tree;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        capacyTimer += Time.deltaTime;

        if (capacyTimer > growthTime)
        {           
            
            Debug.Log("나무 생성");

            GameObject go = Instantiate(Tree, this.transform.position, Quaternion.identity);

            go.transform.parent = ResourceManager.Instance.parentTransform;

            Destroy(gameObject);
        }
    }


}
