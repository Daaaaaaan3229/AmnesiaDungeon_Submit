using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleAttackArea : MonoBehaviour
{
    private GameObject enemyParent;
    private GameObject Player;

    private void Start()
    {
        // 親オブジェクトを取得
        enemyParent = transform.parent.gameObject;

        // プレイヤーオブジェクトをタグで取得
        Player = GameObject.FindWithTag("Player");
    }


    private void OnTriggerEnter(Collider other)
    {
        // プレイヤーが範囲に入った時
        if (other.gameObject == Player)
        {
            enemyParent.GetComponent<TurtleController>().Attack();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // プレイヤーが範囲から出た時
        if (other.gameObject == Player)
        {
            enemyParent.GetComponent<TurtleController>().StopAttack();
        }
    }
}
