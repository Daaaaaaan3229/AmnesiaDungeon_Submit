using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusDisplay : MonoBehaviour
{
    [Header("ステータス表示用のテキスト")]
    [SerializeField]
    private Text status;

    private PlayerStatusControl statusControl;

    private void Start()
    {
        statusControl = GetComponent<PlayerStatusControl>();
        StatusUpdate();
    }

    /// <summary>
    /// ステータス表示の更新
    /// </summary>
    public void StatusUpdate()
    {
        status.text = $"体力 : {statusControl.GetPlayerStatusSO("current").Get("hp").ToString("F0")} / {statusControl.GetPlayerStatusSO("max").Get("hp").ToString("F0")}\n" +
            $"スタミナ : {statusControl.GetPlayerStatusSO("current").Get("st").ToString("F0")} / {statusControl.GetPlayerStatusSO("max").Get("st").ToString("F0")}\n" +
            $"攻撃力 : {statusControl.GetPlayerStatusSO("current").Get("atk").ToString("F0")} / {statusControl.GetPlayerStatusSO("max").Get("atk").ToString("F0")}\n" + 
            $"お金 : {statusControl.GetPlayerStatusSO("current").Get("gold").ToString("F0")}\n" +
            $"経験値 : {statusControl.GetPlayerStatusSO("current").Get("exp").ToString("F0")}";
    }
}
