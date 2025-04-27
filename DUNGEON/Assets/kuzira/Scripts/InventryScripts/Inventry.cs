using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Inventry : MonoBehaviour
{
    public static Inventry instance;
    private InventryUI inventryUI;
    private int index;
    public ItemList items;                         //インベントリのアイテム管理 
    private const int MAX_ITEMCOUNT = 16;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        inventryUI = GetComponent<InventryUI>();
        inventryUI.UpdateUI();
    }

    /// <summary>
    /// アイテムの追加
    /// </summary>
    /// <param name="item">追加するアイテムのScriptableObject</param>
    public void Add(Item item)
    {
        index = items.GetItems().IndexOf(null);
        //アイテムスロットに空きがあれば新しくスロットに追加
        if (index != -1)
        {
            items.AddItem(item);
        }
        else Debug.Log("もう持てません");
        inventryUI.UpdateUI();
    }

    /// <summary>
    /// アイテムの削除
    /// </summary>
    /// <param name="item">削除するアイテムのScriptableObject</param>
    public void Remove(int itemNum)
    {
        //該当するアイテムのインデックスを探すのではなく、スロットの番号に該当するアイテムを削除する

        //index = items.GetItems().IndexOf(item);
        items.RemoveItem(itemNum);
        inventryUI.UpdateUI();
    }

    public void UpdateUI()
    {
        inventryUI.UpdateUI();
    }

    public Slot[] GetSlotList()
    {
        return inventryUI.GetSlotList();
    }
}
