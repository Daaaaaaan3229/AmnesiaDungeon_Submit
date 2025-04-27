using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Register : MonoBehaviour
{
    public static Register instance;
    [SerializeField]
    [Header("登録したアイテムを記録するためのScriptableObject")]
    private ItemList items;
    private RegisterUI registerUI;

    private const int MAX_REGISTER = 4;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        registerUI = GetComponent<RegisterUI>();
        if(items != null)registerUI.UpdateRegisterUI();
    }

    /// <summary>
    /// アイテムを受け取って、レジスタースロットに登録
    /// </summary>
    /// <param name="newItem"></param>
    public void RegisterItem(Item newItem, int newSlotNum)
    {
        //アイテムが登録されているかを確認する
        int existingIndex = Register.instance.GetNumList().FindIndex(num => num == newSlotNum);

        if (existingIndex != -1)
        {
            return;
        }

        if (items.GetItems().Count == MAX_REGISTER && items.GetItems().IndexOf(null) == -1) //アイテム数がMAXかつ空欄がない場合
        {
            Debug.Log("登録できるアイテムの上限を超えています。");
            return;
        }


        items.AddItem(newItem, newSlotNum);
        registerUI.UpdateRegisterUI();
    }

    public void RemoveItem(int num)
    {
        items.GetItems()[num] = null;
        items.ResetItemuSlotNumList(num);

        registerUI.UpdateRegisterUI();
    }

    public List<Item> GetItemList()
    {
        return items.GetItems();
    }

    /// <summary>
    /// itemListから、itemNumListをもってくる
    /// </summary>
    /// <returns></returns>
    public List<int> GetNumList()
    {
        return items.GetItemSlotNumList();
    }
}
