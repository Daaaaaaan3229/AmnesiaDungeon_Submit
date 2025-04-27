using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestMonsterController : EnemyController, IDamagable
{
    void Start()
    {
        Initialize();
        m_navMeshAgent.isStopped = true;//プレイヤーが近づくまでは静止
    }

    void Update()
    {
        //目的地に到達したかを判定する
        if (ReachDestination())
        {
            //達成したら新しい目的地を設定する
            SetDestination();
        }
        SetAnimation();

        //プレイヤーが近づくと動き出す
        if(isRun)
        {
            m_navMeshAgent.isStopped = false;
        }
        //動き出すと常にプレイヤーを追いかける
        if(!m_navMeshAgent.isStopped)
        {
            m_navMeshAgent.SetDestination(playerObj.transform.position);
        }
    }
}
