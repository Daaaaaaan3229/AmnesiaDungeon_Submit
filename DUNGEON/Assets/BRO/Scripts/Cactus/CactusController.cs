using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CactusController : EnemyController
{
    [SerializeField] [Header("弾オブジェクト")] protected GameObject BulletObject;
    private GameObject BulletClone;

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
            //このモンスターはプレイヤーが近づくと立ち止まる
            m_navMeshAgent.isStopped = true;
            transform.LookAt(playerObj.transform.position);
        }
        else
        {
            m_navMeshAgent.isStopped = false;
        }
        SetAnimation();
    }

    // 弾を発射するメソッド
    public void Bullet()
    {
        audioSource.PlayOneShot(attackSound);
        BulletClone = Instantiate(BulletObject, this.transform.position, transform.rotation);
        ShotScript s = BulletClone.GetComponent<ShotScript>();
        s.SetShooter(this); // ShotScriptにCactusControllerの参照を渡す
        Destroy(BulletClone, 2.0f);
        AttackEffect = s.AttackEffect;
    }
}
