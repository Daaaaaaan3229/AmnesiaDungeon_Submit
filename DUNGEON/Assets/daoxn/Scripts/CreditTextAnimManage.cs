using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditTextAnimManage : MonoBehaviour
{
    [Header("EndStorySenarioManager")]
    [SerializeField] private EndStorySenarioManager endStorySenarioManager; 

    /// <summary>
    /// クレジットが終了した処理をアニメーションイベントから呼び出す
    /// </summary>
    public void OnCreditFinish()
    {
        endStorySenarioManager.FinishEndCredit();
    }
}
