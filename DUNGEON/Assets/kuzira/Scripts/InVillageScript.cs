using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InVillageScript : MonoBehaviour
{
    [Header("村のシーンにのみ配置してください")]
    private PlayerStatusControl psCon;
    private void Start()
    {
        psCon = PlayerStatusControl.instance;
        psCon.SetInVillage(true);
    }
}
