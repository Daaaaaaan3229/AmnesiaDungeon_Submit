using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldExpDisplay : MonoBehaviour
{
    [SerializeField] [Header("お金、経験値表示用")] private Text ge_text;
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
        ge_text.text = $"お金 : {psCon.GetPlayerStatusSO("current").Get("gold").ToString("F0")}\n" +
            $"経験値 : {psCon.GetPlayerStatusSO("current").Get("exp").ToString("F0")}";
    }
}
