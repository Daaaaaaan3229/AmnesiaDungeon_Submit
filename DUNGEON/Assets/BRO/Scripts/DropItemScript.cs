using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemScript : MonoBehaviour
{
    [SerializeField]
    [Header("倒した時に落とす経験値")]
    private float exp;
    [SerializeField]
    [Header("倒した時に落とすお金")]
    private float gold;

    [SerializeField]
    [Header("FloorNumSaveスクリタブルオブジェクト")]
    private FloorNumSave floorNumSave;
    
    private PlayerStatusControl psCon;  //PlayerStatusControlのインスタンスを参照する

    private int floorNum;

    private void Start()
    {
        psCon = PlayerStatusControl.instance;

        //階層分、経験値とお金を増加させる
        floorNum = floorNumSave.GetCurrentFloorNum();
        exp *= 1 + floorNum * 2f;
        gold *= 1 + floorNum * 2f;
    }

    /// <summary>
    /// 経験値、お金を渡す
    /// モンスターがやられた時に呼び出される。
    /// </summary>
    public void Drop()
    {
        psCon.StatusCalculate("exp", exp);
        Debug.Log(exp + "の経験値を獲得");
        psCon.StatusCalculate("gold", gold);
        Debug.Log(gold + "のお金を獲得");
    }

    public void DropPlayer()
    {
        int dic = -(floorNum + 1) * 100;
        if (psCon.GetPlayerStatusSO("current").Get("exp") + dic >= 0)psCon.StatusCalculate("exp", dic);
        else psCon.StatusCalculate("exp", -psCon.GetPlayerStatusSO("current").Get("exp"));
        Debug.Log(dic + "の経験値を失った");

        if (psCon.GetPlayerStatusSO("current").Get("gold") + dic >= 0) psCon.StatusCalculate("gold", dic);
        else psCon.StatusCalculate("gold", -psCon.GetPlayerStatusSO("current").Get("gold"));
        Debug.Log(dic + "のお金を失った");
    }
}
