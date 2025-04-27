using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemInfoDisplay : MonoBehaviour
{
    [SerializeField]
    [Header("�����������A�C�e��")]
    private Item item;

    private Text text;  //�\���p�̃e�L�X�g
    private string effect; //���ʂ̊T�v�@HP�񕜁@�X�^�~�i����A�b�v�Ȃ�
    private void Start()
    {
        text = GetComponent<Text>();
        switch (item.GetCurrentState())
        {
            case Effect_List.HP_Recover:
                effect = "HP����";
                break;
            case Effect_List.ST_Recover:
                if (item.GetTimeState() == Effect_Time.Instant) effect = "�X�^�~�i����";
                else if (item.GetTimeState() == Effect_Time.Continuation) effect = "�X�^�~�i����A�b�v";
                break;
            case Effect_List.INVINCIBLE:
                effect = "���G";
                break;
            case Effect_List.ATK_UP:
                effect = "�U���͂�������";
                break;
        }
        GetInfo();
    }

    private void GetInfo()
    {
        if(item.GetTimeState() == Effect_Time.Instant)
        {
            text.text = $"{item.GetName()} : {item.GetGold().ToString("F0")} Gold\n" +
                $"{item.GetValue()} ���� {effect}" ;
        }
        else if(item.GetTimeState() == Effect_Time.Continuation)
        {
            text.text = $"{item.GetName()} : {item.GetGold().ToString("F0")} Gold\n" +
                $"{item.GetTime()} �b�� {effect} ";
        }
        else if(item.GetTimeState() == Effect_Time.Next)
        {
            text.text = $"{item.GetName()} : {item.GetGold().ToString("0F")} Gold\n" +
                $"���ɍU�������ۂɗ^����_���[�W�� {item.GetValue()} �ɂ���B";
        }
    }

}
