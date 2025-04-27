using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterSlot : MonoBehaviour
{
    private Item item;
    private Slot[] slots;
    private RegisterUI registerUI;

    [SerializeField]
    [Header("レジスターアイテムリスト")]
    private ItemList registerItemList;

    private void Start()
    {
        registerUI = GetComponentInParent<RegisterUI>();
    }
    public void RegisterItem(Item registerItem)
    {
        item = registerItem;
        GetComponent<Image>().sprite = item.GetIcon();
    }

    public void Clear()
    {
        item = null;
        GetComponent<Image>().sprite = null;
    }

    /// <summary>
    /// レジスターアイテムを使用した時の処理
    /// </summary>
    /// <returns></returns>
    public bool UseRegisterItem(int index)
    {
        bool result;
        slots = Inventry.instance.GetSlotList();

        //result = slots[Inventry.instance.items.GetItemSlotNum().IndexOf[item]].UseItem(); //アイテムを使用できたときTrue アイテムを使用できなかったとき : False


        result = slots[registerItemList.GetItemSlotNum(index)].UseItem();//アイテムそのもので探すのではなく対応するスロット番号で処理をする
        if (result == false) return false;
        else return true;
    }

    public Item GetItem()
    {
        return item;
    }
}
