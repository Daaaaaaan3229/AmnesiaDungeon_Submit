using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTitleAnimManage : MonoBehaviour
{
    [Header("EndStorySenarioManager")]
    [SerializeField] private EndStorySenarioManager endStorySenarioManager;

    /// <summary>
    /// タイトル画像の演出の終わりをアニメーションイベントから呼び出す
    /// </summary>
    public void OnTitleFinish()
    {
        endStorySenarioManager.OnCredit();
    }
}
