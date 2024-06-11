using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AIState
{
    Idle,
    Wandering,
    Attacking
}

public class NightEnemy : MonoBehaviour, IDamageable
{
    public AudioClip nightEnemySoundClip;
    private AudioSource audioSource;
    private Rigidbody _rigidbody;

    [Header("Stats")]
    public int health;
    public float walkSpeed;
    public float runSpeed;

    [Header("AI")]
    public NavMeshAgent agent;
    public float detectDistance;//목표지점 까지의 거리
    private AIState aiState; //AI 상태 이넘

    [Header("wandering")]
    public float minWanderDistance;
    public float maxWanderDistance;
    public float minWanderWaitTime;
    public float maxWanderWaitTime;

    [Header("Combat")]
    public int damage;
    public float attackRate;
    private float lastAttackTime;
    public float attackDistance;

    private float targetDistance; // 타겟과의 거리
    private GameObject target; // 현재 타겟을 저장할 변수

    public float fieldOfView = 120f;

    private Animator animator;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        SetState(AIState.Wandering);
    }

    void Update()
    {
        if (target == null || target.GetComponent<IDamageable>() == null)
        {
            FindTarget();
        }

        if (Enemy.instance.dayNight.time <= 0.25 || Enemy.instance.dayNight.time >= 0.75)
        {
            if (target != null)
            {
                targetDistance = Vector3.Distance(transform.position, target.transform.position);

                animator.SetBool("Moving", aiState != AIState.Idle);

                switch (aiState)
                {
                    case AIState.Idle:
                    case AIState.Wandering:
                        PassiveUpdate();
                        break;
                    case AIState.Attacking:
                        AttackingUpdate();
                        break;
                }
            }
            else
            {
                SetState(AIState.Wandering);
            }

        }
        else
        {
            Debug.Log("여기임");
            Debug.Log(Enemy.instance.dayNight.time);
            Die();
        }

    }


    public void SetState(AIState state)
    {
        aiState = state;

        switch (aiState)
        {
            case AIState.Idle:
                agent.speed = walkSpeed;
                agent.isStopped = true;
                break;
            case AIState.Wandering:
                agent.speed = walkSpeed;
                agent.isStopped = false;
                break;
            case AIState.Attacking:
                agent.speed = runSpeed;
                agent.isStopped = false;
                break;
        }

        animator.speed = agent.speed / walkSpeed;
    }

    void PassiveUpdate()
    {
        if (aiState == AIState.Wandering && agent.remainingDistance < 0.1f)
        {
            SetState(AIState.Idle);
            Invoke("wanderToNewLocation", Random.Range(minWanderWaitTime, maxWanderWaitTime));
        }

        if (targetDistance < detectDistance)
        {
            SetState(AIState.Attacking);
        }
    }

    void wanderToNewLocation()
    {
        if (aiState != AIState.Idle) return;

        SetState(AIState.Wandering);
        agent.SetDestination(GetWanderLocation());
    }

    private Vector3 GetWanderLocation()
    {
        NavMeshHit hit;

        NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);

        int i = 0;

        while (Vector3.Distance(transform.position, hit.position) < detectDistance)
        {
            NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);
            i++;
            if (i == 30) break;
        }

        return hit.position;
    }

    void AttackingUpdate()
    {
        if (target != null)
        {
            targetDistance = Vector3.Distance(transform.position, target.transform.position);

            if (targetDistance < attackDistance && IsTargetInFieldOfView())
            {
                agent.isStopped = true;
                if (Time.time - lastAttackTime > attackRate)
                {
                    lastAttackTime = Time.time;
                    IDamageable damageableTarget = target.GetComponent<IDamageable>();
                    if (damageableTarget != null)
                    {
                        damageableTarget.TakePhysicalDamage(damage);
                        animator.speed = 1;
                        audioSource.PlayOneShot(nightEnemySoundClip);
                        float sfxVolume = AudioManager.Instance.sfxVolume;
                        audioSource.volume = sfxVolume;
                        animator.SetTrigger("Attack");
                    }
                }
            }
            else if (targetDistance >= attackDistance)
            {
                agent.isStopped = false;
                agent.SetDestination(target.transform.position);
            }
        }
        else
        {
            // 타겟이 파괴되었을 때 상태를 Wandering으로 변경
            SetState(AIState.Wandering);
        }
    }


    private bool IsTargetInFieldOfView()
    {
        if (target == null) return false;

        Vector3 directionToTarget = target.transform.position - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToTarget);
        return angle < fieldOfView * 0.5f;
    }

    private void FindTarget()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");

        List<GameObject> potentialTargets = new List<GameObject>();
        potentialTargets.AddRange(players);
        potentialTargets.AddRange(buildings);

        float closestDistance = float.MaxValue;
        GameObject closestTarget = null;

        foreach (GameObject obj in potentialTargets)
        {
            float distance = Vector3.Distance(transform.position, obj.transform.position);
            if (distance < detectDistance && distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = obj;
            }
        }

        target = closestTarget;
    }




    private void Die()
    {
        agent.enabled = false;
        MonsterPool.Instance.ReturnToPool("Enemy", gameObject);
    }

    public void TakePhysicalDamage(int damage)
    {
        health -= damage;
        Debug.Log("몬스터맞음");
        _rigidbody.AddForce(-transform.forward * 100f, ForceMode.Impulse);
        if (health <= 0)
        {
            Die();
            Debug.Log("몬스터죽음");
        }
    }
}