using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CostDisplay : MonoBehaviour
{
    private PlayerStatusControl psCon;  //ステ振りに
    private Text text;  //ステ振りボタンに表示するテキスト
    [SerializeField]
    [Header("上昇するステータスを入力 : 攻撃力 HP スタミナ")]
    private string status;
    private void Start()
    {
        psCon = PlayerStatusControl.instance;
        text = GetComponent<Text>();
        GetInfo();
    }

    private void GetInfo()
    {
        if (status == "攻撃力")
        {
            text.text = $"{Mathf.Abs(psCon.GetCost("atk")).ToString("F0")} exp を消費して\n" +
                $"{status} が {psCon.GetGain("atk").ToString("F0")} だけ上昇";
        }
        else if(status == "HP")
        {
            text.text = $"{Mathf.Abs(psCon.GetCost("hp")).ToString("F0")} exp を消費して\n" +
                $"{status} が {psCon.GetGain("hp").ToString("F0")} だけ上昇";
        }
        else if(status == "スタミナ")
        {
            text.text = $"{Mathf.Abs(psCon.GetCost("st")).ToString("F0")} exp を消費して\n" +
                $"{status} が {psCon.GetGain("st").ToString("F1")} だけ上昇";
        }

    }

}
