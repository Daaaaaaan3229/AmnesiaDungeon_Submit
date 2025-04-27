using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterItemUse : MonoBehaviour
{
    private RegisterSlot[] slots;
    private PlayerStatusControl psCon;
    private Register register;
    private bool result;
    private RegisterUI registerUI;
    private void Start()
    {
        slots = GetComponentsInChildren<RegisterSlot>();
        register = Register.instance;
        registerUI = GetComponent<RegisterUI>();
        psCon = PlayerStatusControl.instance;
    }
    void Update()
    {
        if(psCon.GetInVillage() == false)//村で使えないようにする
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && slots[0].GetItem() != null)
            {
                result = slots[0].UseRegisterItem(0);
                if (result == true) register.RemoveItem(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) && slots[1].GetItem() != null)
            {
                result = slots[1].UseRegisterItem(1);
                if (result == true) register.RemoveItem(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3) && slots[2].GetItem() != null)
            {
                result = slots[2].UseRegisterItem(2);
                if (result == true) register.RemoveItem(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4) && slots[3].GetItem() != null)
            {
                result = slots[3].UseRegisterItem(3);
                if (result == true) register.RemoveItem(3);
            }
            registerUI.UpdateRegisterUI();
        }
    }
}
