using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChildScript : EnemyController
{
    private MushroomController parent; // 弾を発射したMushroomControllerの参照を保持

    // MushroomControllerの参照を設定するメソッド
    public void SetParent(MushroomController mushroomController)
    {
        parent = mushroomController;
    }

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

    protected override void Die()
    {
        //移動を止める
        m_navMeshAgent.isStopped = true;
        rb.constraints = RigidbodyConstraints.FreezeRotationY;

        //削除対象に自分を設定
        Destroy(this.gameObject, 0.4f);
    }

}

