using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSlot : MonoBehaviour
{
    [SerializeField]
    private Slot slot;
    private Item item;
    private int slotNum;
    
    //登録ボタンを押して、アイテムを登録する
    public void SetSlot()
    {
        item = slot.GetItem();
        slotNum = slot.GetSlotNum();//登録するアイテムのスロット番号を取得
        Register.instance.RegisterItem(item, slotNum);
    }
}
