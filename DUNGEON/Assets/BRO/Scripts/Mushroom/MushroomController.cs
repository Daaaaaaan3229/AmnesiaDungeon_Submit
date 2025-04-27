using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomController : EnemyController
{
    [SerializeField] [Header("子分オブジェクト")] protected GameObject ChildObject;
    private GameObject ChildClone;
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

    /// <summary>
    /// オーバーライドし、自滅する処理を追加
    /// </summary>
    /// <param name="damage"></param>
    public override void AddAtk()
    {
        // 親クラスの AddAtk メソッドを呼び出し、基本的な処理を行う
        base.AddAtk();

        // 攻撃を当てた後死ぬ
        AddDamage(100);
    }

    /// <summary>
    /// オーバーライドし、ミニキノコの生成処理を追加
    /// </summary>
    /// <param name="damage"></param>
    protected override void Die()
    {
        // 親クラスの Die メソッドを呼び出し、基本的な処理を行う
        base.Die();

        // ミニキノコ生成の追加処理
        for (int i=0; i<3; i++)
        {
            Clone();
        }
    }

    public void Clone()
    {
        ChildClone = Instantiate(ChildObject, this.transform.position, transform.rotation);
        ChildScript c = ChildClone.GetComponent<ChildScript>();
        c.SetParent(this); // ChildScriptにMushroomControllerの参照を渡す
        Debug.Log("子キノコ生成しました");
        //プレイヤーオブジェクトを渡す
        c.SetParameter(playerObj, floorNum);
    }
}
