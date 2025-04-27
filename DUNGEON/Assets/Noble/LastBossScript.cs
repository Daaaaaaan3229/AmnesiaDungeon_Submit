using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastBossScript : BossMoveScript
{
    [SerializeField] private GameObject _MRAEffect;
    [SerializeField] private Transform _MRAEffectTarget;

    [SerializeField] private GameObject _LRAEffect;
    [SerializeField] private Transform _LRAEffectTarget;

    [SerializeField] private GameObject _SRACollider;
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        EnemyMove();

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
        //await UniTask.Delay(TimeSpan.FromSeconds(3f));
        while (true)
        {
            
            ChangeState(EnemyState.Idle);
            await UniTask.Delay(TimeSpan.FromSeconds(waitTime));


            if (!rangeNotifications[inLR] && !rangeNotifications[inSR] && !rangeNotifications[inMR])
            {

                ChangeState(EnemyState.Chase);


                bool reachedAttackRange = await UniTask.WhenAny(
                    UniTask.WaitUntil(() => IsDistanceLessThan(longAttackRange)),
                    UniTask.Delay(TimeSpan.FromSeconds(chaseTime))
                ) == 0;


                if (reachedAttackRange)
                {
                    await WaitForAnimationEnd(animator);
                    ChangeState(EnemyState.Attack);
                }
            }


            if (rangeNotifications[inSR] || rangeNotifications[inMR] || rangeNotifications[inLR])
            {
                await WaitForAnimationEnd(animator);
                ChangeState(EnemyState.Attack);
                await UniTask.Delay(TimeSpan.FromSeconds(waitTime));
            }
        }
    }

    protected override void Attack()
    {
        if (IsDistanceLessThan(shortAttackRange))
        {
            animator.SetTrigger(SRAParamHash);
        }
        else if (IsDistanceLessThan(middleAttackRange, shortAttackRange))
        {
            animator.SetTrigger(MRAParamHash);

        }
        else if (IsDistanceLessThan(longAttackRange, middleAttackRange))
        {
            animator.SetTrigger(LRAParamHash);

        }
    }

    public void SRAttack()
    {
        _SRACollider.SetActive(true);
    }

    public void FinishSRAttack()
    {
        _SRACollider.SetActive(false); 
    }

    public void MRAttack()
    {
        Instantiate(_MRAEffect, _MRAEffectTarget.position, transform.rotation);
        _audioManager.PlayAudio(3);
    }

    public void LRAttack()
    {
        Instantiate(_LRAEffect, _LRAEffectTarget.position, transform.rotation);
        _audioManager.PlayAudio(3);
    }

    protected override void SpecialAttack()
    {
        
    }

    public override void Death()
    {
        animator.SetBool(isDeadParamhash, true);
        _audioManager.PlayAudio(4);
    }

    public override void Damage(float damage)
    {
        _audioManager.PlayAudio(2);
        hp -= damage - def;

        Debug.Log(gameObject.name + "は" + damage + "ダメージを受けた");

        if (hp <= 0)
        {
            ChangeState(EnemyState.Death);
        }
    }

    private async UniTask WaitForAnimationEnd(Animator animator)
    {
        if (animator == null) return;

        var animationState = animator.GetCurrentAnimatorStateInfo(0);

   
        // アニメーションの終了を待機
        while (animator != null && animationState.normalizedTime < 1f)
        {
            await UniTask.Yield();
            animationState = animator.GetCurrentAnimatorStateInfo(0);
            
            if (animator == null || !animator.isActiveAndEnabled)
            {
                return;
            }

        }
    }

    public void Scream()
    {
        _audioManager.PlayAudio(0);
    }

    public void Wing()
    {
        _audioManager.PlayAudio(1);
    }
}
