using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterUI : MonoBehaviour
{
    private RegisterSlot[] rSlot;
    private void Start()
    {
        rSlot = GetComponentsInChildren<RegisterSlot>();
    }
    public void UpdateRegisterUI()
    {
        for(int i = 0 ; i < rSlot.Length; i++)
        {
            if (i < Register.instance.GetItemList().Count && Register.instance.GetItemList()[i] != null)
            {
                rSlot[i].RegisterItem(Register.instance.GetItemList()[i]);
            }
            else
            {
                rSlot[i].Clear();
            }
        }

        RegisterItemDisplay.instance.UpdateDisplayUI();
    }
}
