using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventryUI : MonoBehaviour
{
    [SerializeField]
    [Header("Slot�̐e�I�u�W�F�N�g(��K�w��̃I�u�W�F�N�g)")]
    private Transform InventryPanel;

    private Slot[] slots;
    void Awake()
    {
        slots = InventryPanel.GetComponentsInChildren<Slot>();
    }

    /// <summary>
    /// �A�C�e������ɓ��ꂽ��A�g�����肵���Ƃ��A�����X�V���ĕ\����ύX����.
    /// </summary>
    public void UpdateUI()
    {
        Debug.Log("UpdateUi");
        Debug.Log(Inventry.instance.items.GetItems().Count);
        for (int i = 0; i < slots.Length; i++)
        {
            if (Inventry.instance.items.GetItems()[i] != null)  //�A�C�e����Inventry�̃��X�g�ɓo�^����Ă����
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
