using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemInfoDisplay : MonoBehaviour
{
    [SerializeField]
    [Header("説明を書くアイテム")]
    private Item item;

    private Text text;  //表示用のテキスト
    private string effect; //効果の概要　HP回復　スタミナ上限アップなど
    private void Start()
    {
        text = GetComponent<Text>();
        switch (item.GetCurrentState())
        {
            case Effect_List.HP_Recover:
                effect = "HPを回復";
                break;
            case Effect_List.ST_Recover:
                if (item.GetTimeState() == Effect_Time.Instant) effect = "スタミナを回復";
                else if (item.GetTimeState() == Effect_Time.Continuation) effect = "スタミナ上限アップ";
                break;
            case Effect_List.INVINCIBLE:
                effect = "無敵";
                break;
            case Effect_List.ATK_UP:
                effect = "攻撃力があがる";
                break;
        }
        GetInfo();
    }

    private void GetInfo()
    {
        if(item.GetTimeState() == Effect_Time.Instant)
        {
            text.text = $"{item.GetName()} : {item.GetGold().ToString("F0")} Gold\n" +
                $"{item.GetValue()} だけ {effect}" ;
        }
        else if(item.GetTimeState() == Effect_Time.Continuation)
        {
            text.text = $"{item.GetName()} : {item.GetGold().ToString("F0")} Gold\n" +
                $"{item.GetTime()} 秒間 {effect} ";
        }
        else if(item.GetTimeState() == Effect_Time.Next)
        {
            text.text = $"{item.GetName()} : {item.GetGold().ToString("0F")} Gold\n" +
                $"次に攻撃した際に与えるダメージを {item.GetValue()} にする。";
        }
    }

}
