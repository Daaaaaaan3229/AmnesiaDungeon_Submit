using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲーム中に進行したダンジョンの最大階数を保存するScritableObject
/// </summary>

[CreateAssetMenu(menuName ="FloorNumSave_Scritable")]
public class FloorNumSave : ScriptableObject
{
    [SerializeField]
    private int currentFloorNum;

    [SerializeField]
    private int maxFloorNum;

    /// <summary>
    /// ダンジョン階数を保存する
    /// </summary>
    /// <param name="num"></param>
    public void FloorNumSet(int num)
    {
        currentFloorNum = num;

        //ダンジョン階数が現在保存してる階数より小さければ保存できないようにする
        if(maxFloorNum < num)
        {
            maxFloorNum = num;
        }
    }

    /// <summary>
    ///現在のダンジョン階数をint型で渡す
    /// </summary>
    /// <returns></returns>
    public int GetCurrentFloorNum()
    {
        return currentFloorNum;
    }

    /// <summary>
    /// 最大のダンジョン回数をint型で渡す
    /// </summary>
    /// <returns></returns>
    public int GetMaxFloorNum()
    {
        return maxFloorNum;
    }
}
