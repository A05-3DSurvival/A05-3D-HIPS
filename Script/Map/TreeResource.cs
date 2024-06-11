using UnityEngine;
using UnityEngine.UI;

public class TreeResource : Resource
{
    Animator animator;

    public int  maxCapacy;      // �ִ� ���� Ƚ��

    public float capacyTimer;

    public float growthTime = 30;    // �����ϴµ� �ɸ��� �ð�

    public GameObject felledTree;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Growth();

        TreeFellCheck();
    }
       

    void Growth()
    {
        capacyTimer += Time.deltaTime;

        if (capacyTimer > growthTime)
        {
            capacyTimer = 0;
            capacy++;
            //Debug.Log("�߰� �ڿ� ����");

            if (capacy > maxCapacy)
            {
                capacy = maxCapacy;
            }
        }
    }
    private void TreeFellCheck()
    {
        if ( capacy <= 0)
        {
            animator.SetBool("Death", true);
        }
        else
        {
            animator.SetBool("Death", false);
        }
    }

    // �ִϸ��̼� �̺�Ʈ�� ���
    private void TreeFell()
    {
        GameObject go = Instantiate(felledTree, this.transform.position, Quaternion.identity);

        go.transform.parent = ResourceManager.Instance.parentTransform;

        Destroy(gameObject);
    }
}
