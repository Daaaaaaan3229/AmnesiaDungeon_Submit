using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDetection : MonoBehaviour
{
    protected EnemyController enemyScript; // 親オブジェクトのスクリプトを格納する変数

    private void Start()
    {
        // 親オブジェクトからスクリプトを取得
        enemyScript = transform.parent.gameObject.GetComponent<EnemyController>();

        if (enemyScript == null)
        {
            Debug.LogError("親オブジェクトにスクリプトが見つかりませんでした。");
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        CallAddAtk(other);
    }

    protected void CallAddAtk(Collider other)
    {
        // プレイヤーが範囲に入った時
        if (other.gameObject.CompareTag("Player") && enemyScript != null)
        {
            // 親オブジェクトのスクリプト内のAddAtk()メソッドを呼び出す
            enemyScript.AddAtk();
        }
    }
}
