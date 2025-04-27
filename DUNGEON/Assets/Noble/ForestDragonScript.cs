using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestDragonScript : BossMoveScript
{
    [SerializeField] private Transform fireBallTransform;
    [SerializeField] private GameObject fireBallPrefab;
    [SerializeField] private Transform specialFireBallTransform;
    [SerializeField] private GameObject specialFireBallPrefab;
    [SerializeField] private int breakCount = 3;
    [SerializeField] private GameObject acollider;

    private bool isFlying;
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        EnemyMove();
        isFlying = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!animator.GetBool(isDeadParamhash))
        {
            UpdateDistance();
            UpdateRotation();
            ActState();
        } 
    }

    protected override async void EnemyMove()
    {
        
        await UniTask.Delay(TimeSpan.FromSeconds(3f));
        while (true)
        {
            if (isFlying) return;
            ChangeState(EnemyState.Idle);
            await UniTask.Delay(TimeSpan.FromSeconds(waitTime));


            if (!rangeNotifications[inLR] && !rangeNotifications[inSR])
            {

                ChangeState(EnemyState.Chase);


                bool reachedAttackRange = await UniTask.WhenAny(
                    UniTask.WaitUntil(() => IsDistanceLessThan(shortAttackRange)),
                    UniTask.Delay(TimeSpan.FromSeconds(chaseTime))
                ) == 0;


                if (reachedAttackRange)
                {
                    ChangeState(EnemyState.Attack);
                }
            }


            if (rangeNotifications[inSR] || rangeNotifications[inLR])
            {
                ChangeState(EnemyState.Attack);
                await UniTask.Delay(TimeSpan.FromSeconds(waitTime));
            }
        }
    }

    protected override void Attack()
    {
        if (hp <= hp * (specialAttackHpPercentage / 100) && CheckProbability())
        {
            SpecialAttack();
        }
        else
        {
            if (IsDistanceLessThan(shortAttackRange))
            {
                animator.SetTrigger(SRAParamHash);

            }
            else if (IsDistanceLessThan(longAttackRange,middleAttackRange))
            {
                animator.SetTrigger(LRAParamHash);
            }
        }
    }

    public void FireBallAttack()
    {
        Instantiate(fireBallPrefab, fireBallTransform.position,transform.rotation);
        _audioManager.PlayAudio(3);
    }

    public void SpecialFireBallAttack()
    {
        Instantiate(specialFireBallPrefab, specialFireBallTransform.position, transform.rotation);
        _audioManager.PlayAudio(3);
    }
    public void slashWave()
    {
        acollider.SetActive(true);
    }

    public void slashWaveFinish()
    {
        acollider.SetActive(false);
    }

    protected override async void SpecialAttack()
    {
        isFlying = true;
        animator.SetTrigger(SAParamHash);
        await UniTask.Delay(TimeSpan.FromSeconds(5f));
        isFlying = false;
        EnemyMove();
    }

    public override void Death()
    {
        animator.SetBool(isDeadParamhash, true);
        _audioManager.PlayAudio(4);
    }

    public override void Damage(float damage)
    {
        if (!isFlying)
        {
            _audioManager.PlayAudio(2);
            hp -= damage - def;

            Debug.Log(gameObject.name + "は" + damage + "ダメージを受けた");

            if (hp <= 0)
            {
                ChangeState(EnemyState.Death);
            }
        }
    }
    public void Scream()
    {
        _audioManager.PlayAudio(0);
    }

}
