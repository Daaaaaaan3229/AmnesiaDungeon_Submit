using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cysharp.Threading.Tasks;

public abstract class BossMoveScript : MonoBehaviour
{
    protected Animator animator;
    protected NavMeshAgent navMeshAgent;
    protected bool isStateChanged;
    protected bool isDeath;
    protected Transform targetTransform;
    protected BossAudioManager _audioManager;

    protected const int inLR = 0;
    protected const int inMR = 1;
    protected const int inSR = 2;
    protected bool[] rangeNotifications = { false, false, false };

    [SerializeField] protected float rotationSpeed = 5f;
    [SerializeField] protected float waitTime = 5f;
    [SerializeField] protected float chaseTime = 5f;
    [SerializeField] protected float longAttackRange = 5f;
    [SerializeField] protected float middleAttackRange = 3f;
    [SerializeField] protected float shortAttackRange = 1f;
    [SerializeField] protected float specialAttackHpPercentage; //SpecialAttackが発動するHPの割合  
    [SerializeField] protected float specialPercentage; //SpecialAttackが発動する確率

    protected Transform thisTransform;
    protected static readonly int ActionParamHash = Animator.StringToHash("Action");
    protected static readonly int isDeadParamhash = Animator.StringToHash("isDead");
    protected static readonly int LRAParamHash = Animator.StringToHash("LongRangeAttack");
    protected static readonly int MRAParamHash = Animator.StringToHash("MiddleRangeAttack");
    protected static readonly int SRAParamHash = Animator.StringToHash("ShortRangeAttack");
    protected static readonly int SAParamHash = Animator.StringToHash("SpecialAttack");


    public string bossName {get; protected set;}
    public float hp { get; protected set;}
    public float atk { get; protected set; }
    public float def{  get; protected set; }

    [SerializeField] protected EnemyStatus enemyStatus;
    public enum EnemyState
    {
        Idle,
        Chase,
        Attack,
        SpecialAttack,
        Death,
    }
    public EnemyState state { get; private set; }

    public void ChangeState(EnemyState tempState)
    {
        if (this.state == tempState)
        {
            return;
        }

        this.state = tempState;
        this.isStateChanged = true;
    }

    protected void Initialize()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        targetTransform = GameObject.FindWithTag("Player").transform;
        _audioManager = FindObjectOfType<BossAudioManager>();
        state = EnemyState.Idle;
        isStateChanged = false;
        isDeath = false;
        bossName = enemyStatus.name;
        hp = enemyStatus.Hp;
        atk = enemyStatus.Atk;
        def = enemyStatus.Def;
        thisTransform = this.transform;
    }

    protected void ActState()
    {
        switch (state)
        {
            case EnemyState.Idle:
                if (isStateChanged)
                {
                    isStateChanged = false;
                    Debug.Log("Changed to Idle");
                    animator.SetInteger(ActionParamHash, 0);

                }

                navMeshAgent.isStopped = true;
                break;

            case EnemyState.Chase:
                if (isStateChanged)
                {
                    isStateChanged = false;
                    Debug.Log("Changed to Chase");
                    animator.SetInteger(ActionParamHash, 1);
                }
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(targetTransform.position);
                break;

            case EnemyState.Attack:
                if (isStateChanged)
                {
                    isStateChanged = false;
                    Debug.Log("Changed to Attack");
                    
                        Attack();
                }
                navMeshAgent.isStopped = true;
                break;

            case EnemyState.SpecialAttack:
                if (isStateChanged)
                {
                    isStateChanged = false;
                    //Debug.Log("Changed to Attack");
                    SpecialAttack();


                }
                navMeshAgent.isStopped = true;
                break;

            case EnemyState.Death:
                if(isStateChanged)
                {
                    isStateChanged = false;
                    Death();
                }
                navMeshAgent.isStopped = true;
                break;
        }
    }

    protected bool IsDistanceLessThan(float comparingDistance)
    {
        var distance = (thisTransform.position - targetTransform.position).sqrMagnitude;
        if (comparingDistance * comparingDistance >= distance)
        {
            return true;
        }
        return false;
    }



    protected bool CheckProbability()
    {
        int num = Random.Range(1, 100);
        if(num <= specialPercentage)
        {
            return true;
        }

        return false;
    }

    protected bool IsDistanceLessThan(float outerDistance, float innerDistance)
    {
        // 敵とプレイヤーの距離の二乗を計算
        float distanceSquared = (transform.position - targetTransform.position).sqrMagnitude;

        // 距離が innerDistance 以上かつ outerDistance 以下の場合に true を返す
        return (innerDistance * innerDistance) <= distanceSquared && distanceSquared <= (outerDistance * outerDistance);
    }

    protected void UpdateDistance()
    {
        rangeNotifications[inLR] = IsDistanceLessThan(longAttackRange, middleAttackRange);
        rangeNotifications[inMR] = IsDistanceLessThan(middleAttackRange, shortAttackRange);
        rangeNotifications[inSR] = IsDistanceLessThan(shortAttackRange);
    }

    protected void UpdateRotation()
    {
        Vector3 direction = targetTransform.position - transform.position;

        direction.y = 0;

        if (direction.magnitude > 0 && state == EnemyState.Attack)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,shortAttackRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, middleAttackRange);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, longAttackRange);

    }

    protected abstract void Attack();
    public abstract void Damage(float damage);
    protected abstract void SpecialAttack();
    protected abstract void EnemyMove();
    public abstract void Death();



}
