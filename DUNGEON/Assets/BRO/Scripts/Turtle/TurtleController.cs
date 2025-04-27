using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleController : EnemyController, IDamagable
{
    private bool isAttacked=false; //isAttackを保存
    [SerializeField] [Header("とげ攻撃を当てたときのエフェクト")] protected GameObject TogeAttackEffect;//とげ攻撃用のエフェクト
    void Start()
    {
        Initialize();
    }

    void Update()
    {
        //目的地に到達したかを判定する
        if (ReachDestination())
        {
            //達成したら新しい目的地を設定する
            SetDestination();
        }
        else if (isRun)
        {
            //プレイヤーに向かわせる
            m_navMeshAgent.SetDestination(playerObj.transform.position);
        }

        SetAnimation();

        if (isAttack)
        {
            isAttacked = true;
        }
        if (isAttacked)
        {
            m_navMeshAgent.isStopped = true;
        }
    }

    /// <summary>
    /// オーバーライドし、エフェクトを変更
    /// </summary>
    public override void AddAtk()
    {
        //普段はとげ攻撃、爆発寸前でatkを0にして爆発エフェクトに切り替え
        if(atk != 0)
        {
            //プレイヤーにダメージを与える
            iDamagable.AddDamage(atk);
            TogeAttackEffect.SetActive(true);
            Debug.Log(name + "はプレイヤーに" + atk + "ダメージ与えた");
        } else
        {
            base.AddAtk();
        }
    }

    /// <summary>
    /// 爆発寸前にatk=0にする
    /// </summary>
    public void Atk0()
    {
        atk = 0;
    }
}
