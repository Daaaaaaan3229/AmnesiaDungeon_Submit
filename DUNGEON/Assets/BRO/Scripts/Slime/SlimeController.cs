using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeController : EnemyController
{
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
    }
}
