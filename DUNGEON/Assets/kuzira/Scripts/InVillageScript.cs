using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InVillageScript : MonoBehaviour
{
    [Header("���̃V�[���ɂ̂ݔz�u���Ă�������")]
    private PlayerStatusControl psCon;
    private void Start()
    {
        psCon = PlayerStatusControl.instance;
        psCon.SetInVillage(true);
    }
}
