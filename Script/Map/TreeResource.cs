using UnityEngine;
using UnityEngine.UI;

public class TreeResource : Resource
{
    Animator animator;

    public int  maxCapacy;      // 최대 목재 횟수

    public float capacyTimer;

    public float growthTime = 30;    // 성장하는데 걸리는 시간

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
            //Debug.Log("추가 자원 생성");

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

    // 애니메이션 이벤트로 사용
    private void TreeFell()
    {
        GameObject go = Instantiate(felledTree, this.transform.position, Quaternion.identity);

        go.transform.parent = ResourceManager.Instance.parentTransform;

        Destroy(gameObject);
    }
}
