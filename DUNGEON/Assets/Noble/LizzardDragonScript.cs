using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LizzardDragonScript : BossMoveScript
{

    [SerializeField] private GameObject waveObject;
    [SerializeField] private Transform wavePos;
    [SerializeField] private GameObject acollider;

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

    protected override void Attack()
    {
        if (hp <= hp * (specialAttackHpPercentage / 100) && CheckProbability())
        {
            SpecialAttack();
            _audioManager.PlayAudio(3);
        }
        else
        {
            if (IsDistanceLessThan(shortAttackRange))
            {
                animator.SetTrigger(SRAParamHash);

            }
            else if (IsDistanceLessThan(middleAttackRange))
            {
                animator.SetTrigger(MRAParamHash);
                _audioManager.PlayAudio(3);
            }
        }
    }

    protected override void SpecialAttack()
    {
        animator.SetTrigger(SAParamHash);
    }

    public override void Death()
    {
        animator.SetBool(isDeadParamhash, true);
        _audioManager.PlayAudio(4);
    }

    protected override async void EnemyMove()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(3f));
        while (true)
        {
 
            ChangeState(EnemyState.Idle);
            await UniTask.Delay(TimeSpan.FromSeconds(waitTime));

 
            if (rangeNotifications[inLR])
            {
    
                ChangeState(EnemyState.Chase);


                bool reachedAttackRange = await UniTask.WhenAny(
                    UniTask.WaitUntil(() => IsDistanceLessThan(middleAttackRange)),
                    UniTask.Delay(TimeSpan.FromSeconds(chaseTime))
                ) == 0;


                if (reachedAttackRange)
                {
                    ChangeState(EnemyState.Attack);
                }
            }

   
            if (rangeNotifications[inMR] || rangeNotifications[inSR])
            {
                ChangeState(EnemyState.Attack);
                await UniTask.Delay(TimeSpan.FromSeconds(waitTime));
            }
        }
    }

    public void AttackWave()
    {
        Instantiate(waveObject, wavePos.position, transform.rotation);
    }

    public void slashWave()
    {
        acollider.SetActive(true);
    }

    public void slashWaveFinish()
    {
        acollider.SetActive(false);
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

    public void Scream()
    {
        _audioManager.PlayAudio(0);
    }



}
