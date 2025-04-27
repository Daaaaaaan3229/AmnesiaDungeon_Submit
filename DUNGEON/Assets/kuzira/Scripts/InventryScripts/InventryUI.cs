using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventryUI : MonoBehaviour
{
    [SerializeField]
    [Header("Slotの親オブジェクト(一階層上のオブジェクト)")]
    private Transform InventryPanel;

    private Slot[] slots;
    void Awake()
    {
        slots = InventryPanel.GetComponentsInChildren<Slot>();
    }

    /// <summary>
    /// アイテムを手に入れたり、使ったりしたとき、情報を更新して表示を変更する.
    /// </summary>
    public void UpdateUI()
    {
        Debug.Log("UpdateUi");
        Debug.Log(Inventry.instance.items.GetItems().Count);
        for (int i = 0; i < slots.Length; i++)
        {
            if (Inventry.instance.items.GetItems()[i] != null)  //アイテムがInventryのリストに登録されていれば
            {
                slots[i].AddItem(Inventry.instance.items.GetItems()[i]);
            }
            else
            {
                slots[i].ClearItem();
            }
        }
    }

    public Slot[] GetSlotList()
    {
        return slots;
    }

}
