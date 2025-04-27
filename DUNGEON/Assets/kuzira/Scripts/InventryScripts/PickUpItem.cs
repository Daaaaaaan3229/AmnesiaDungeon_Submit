using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PickUpItem : MonoBehaviour
{
    //Itemデータを入れる
    [SerializeField]
    [Header("ItemのScriptableObject")]
    private Item item;

    private PlayerStatusControl psCon;  //PlayerStatusControlのインスタンスを参照する
    private GoldExpDisplay geDis;       //GoldExpDisplayのインスタンスを参照する

    private const int MAX_ITEM = 16;

    private float itemGold;//アイテムの金額

    void Start()
    {
        //設定したアイコンを表示させる
        GetComponent<Image>().sprite = item.GetIcon();

        //アイテムの金額の取得
        itemGold = item.GetGold();

        //プレイヤーステータスのインスタンスを取得
        psCon = PlayerStatusControl.instance;

        geDis = GoldExpDisplay.instance;
    }

    //インベントリにアイテムを追加
    public void PickUp()
    {
        if(Inventry.instance.items.GetItems().IndexOf(null) != -1 && psCon.GetPlayerStatusSO("current").Get("gold") >= itemGold )
        {
            psCon.StatusCalculate("gold", -itemGold);
            geDis.Display();
            Inventry.instance.Add(item);
        }
    }
}
