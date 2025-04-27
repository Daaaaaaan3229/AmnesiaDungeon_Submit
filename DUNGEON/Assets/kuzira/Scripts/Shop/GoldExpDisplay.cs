using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldExpDisplay : MonoBehaviour
{
    [SerializeField] [Header("�����A�o���l�\���p")] private Text ge_text;
    PlayerStatusControl psCon;
    static public GoldExpDisplay instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        psCon = PlayerStatusControl.instance;
    }
    public void Display()
    {
        ge_text.text = $"���� : {psCon.GetPlayerStatusSO("current").Get("gold").ToString("F0")}\n" +
            $"�o���l : {psCon.GetPlayerStatusSO("current").Get("exp").ToString("F0")}";
    }
}
