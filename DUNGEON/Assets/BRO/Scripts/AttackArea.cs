using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    private EnemyController enemyController;

    private void Start()
    {
        //親オブジェクトのEnemyControllerコンポネントを取得
        enemyController = transform.parent.gameObject.GetComponent<EnemyController>();

        //EnemyControllerを取得してからColliderをオンにする
        GetComponent<Collider>().enabled = true;

    }

    private void OnTriggerEnter(Collider other)
    {
        // プレイヤーが範囲に入った時
        //タグでPlayerであるかを判別
        if (other.CompareTag("Player"))
        {
            enemyController.Attack();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // プレイヤーが範囲から出た時
        if (other.CompareTag("Player"))
        {
            enemyController.StopAttack();
        }
    }
}
