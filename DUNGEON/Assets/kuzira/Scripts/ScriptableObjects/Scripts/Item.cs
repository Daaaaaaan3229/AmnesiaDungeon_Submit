using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObjects/Create Item")]    

public class Item : ScriptableObject
{
    [SerializeField]
    [Header("アイテムの名前")]
    new private string name = "Item Name";

    [SerializeField]
    [Header("アイテムのアイコン")]
    private Sprite icon = null;

    [SerializeField]
    [Header("アイテムの効果の選択")]
    private Effect_List currentState;

    [SerializeField]
    [Header("アイテムの効果時間の設定。即時 or 持続")]
    private Effect_Time timeState;

    [SerializeField]
    [Header("アイテムの効果量を設定")]
    private float value;

    [SerializeField]
    [Header("アイテムの効果時間を設定\nアイテムの効果がが即時の物なら設定の必要なし。")]
    private float time;

    [SerializeField]
    [Header("アイテムを購入する際に必要な金額")]
    private float gold;

    public Effect_List GetCurrentState()
    {
        return currentState;
    }

    public Effect_Time GetTimeState()
    {
        return timeState;
    }

    public Sprite GetIcon()
    {
        return icon;
    }

    public float GetValue()
    {
        return value;
    }

    public float GetTime()
    {
        return time;
    }

    public string GetName()
    {
        return name;
    }

    public float GetGold()
    {
        return gold;
    }
}
public enum Effect_List
{
    HP_Recover,
    ST_Recover,
    MAG_Recover,
    ATK_UP,
    INVINCIBLE
}

public enum Effect_Time
{
    Instant,
    Continuation,
    Next
}
