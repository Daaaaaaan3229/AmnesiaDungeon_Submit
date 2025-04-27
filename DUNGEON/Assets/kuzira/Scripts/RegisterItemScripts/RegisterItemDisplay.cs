using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterItemDisplay : MonoBehaviour
{
    private DisplaySlot[] dSlots;
    public static RegisterItemDisplay instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        dSlots = GetComponentsInChildren<DisplaySlot>();

        Debug.Log($"DisplaySlots initialized: {dSlots.Length}");

    }

    public void UpdateDisplayUI()
    {
        if(dSlots == null)
        {
            dSlots = GetComponentsInChildren<DisplaySlot>();
        }

        for(int i = 0 ; i < dSlots.Length ; i++)
        {
            if (i < Register.instance.GetItemList().Count && Register.instance.GetItemList()[i] != null) dSlots[i].Display(Register.instance.GetItemList()[i]);
            else dSlots[i].Clear();
        }
    }
}
