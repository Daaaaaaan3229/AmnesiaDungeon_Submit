using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedleDragonScript : BossMoveScript
{
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
        UpdateDistance();
        UpdateRotation();
        ActState();
    }

    protected override async void EnemyMove()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(3f));
        while (true)
        {
            if (isFlying) return;
            ChangeState(EnemyState.Idle);
            await UniTask.Delay(TimeSpan.FromSeconds(waitTime));


            if (rangeNotifications[inSR] || rangeNotifications[inMR] || rangeNotifications[inLR])
            {
                ChangeState(EnemyState.Attack);
                await UniTask.Delay(TimeSpan.FromSeconds(waitTime));
            }
            else
            {
                ChangeState(EnemyState.Chase);


                bool reachedAttackRange = await UniTask.WhenAny(
                    UniTask.WaitUntil(() => IsDistanceLessThan(longAttackRange)),
                    UniTask.Delay(TimeSpan.FromSeconds(chaseTime))
                ) == 0;


                if (reachedAttackRange)
                {
                    ChangeState(EnemyState.Attack);
                }
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
            else if (IsDistanceLessThan(middleAttackRange, shortAttackRange))
            {
                animator.SetTrigger(MRAParamHash);
            }
            else
            {
                animator.SetTrigger(LRAParamHash);
            }
        }
    }

    protected override void SpecialAttack()
    {
        
    }

    public override void Death()
    {
        
    }

    public override void Damage(float damage)
    {

    }

}
