using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ItemList", menuName = "ScriptableObjects/Create ItemList")]
public class ItemList:ScriptableObject
{
    [SerializeField]
    private List<Item> items = new List<Item>();

    [SerializeField]
    //レジスターアイテムに対応するスロット番号を保存
    private List<int> itemSlotNum = new List<int>();

    /// <summary>
    /// アイテムを保存する
    /// レジスターアイテムの場合は、slotNumを指定して保存する必要がある。それ以外は-1。
    /// </summary>
    /// <param name="_item"></param>
    /// <param name="slotNum"></param>
    public void AddItem(Item _item, int slotNum = -1)
    {
        for(int i = 0;i<items.Count;i++)
        {
            if (items[i] == null)
            {
                items[i] = _item;

                //スロット番号が指定された場合は一緒に保存
                if (slotNum != -1)
                {
                    // itemSlotNum のサイズを確認して不足があれば拡張
                    while (itemSlotNum.Count <= i)
                    {
                        itemSlotNum.Add(-1); // 初期値を追加
                    }
                    itemSlotNum[i] = slotNum;
                }

                return;
            }
        }
    }


    public void RemoveItem(int num)
    {
        items[num] = null;
    }


    public List<Item> GetItems()
    {
        return items;
    }

    /// <summary>
    /// itemSlotNumリストを渡す
    /// </summary>
    /// <returns></returns>
    public List<int> GetItemSlotNumList()
    {
        return itemSlotNum;
    }

    /// <summary>
    /// 使用したアイテムのレジスタースロット番号を-1にリセット
    /// </summary>
    /// <param name="num"></param>
    public void ResetItemuSlotNumList(int num)
    {
        itemSlotNum[num] = -1;
    }

    //指定されたアイテム番号を返す
    public int GetItemSlotNum(int index)
    {
        return itemSlotNum[index];
    }
}
