using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CostDisplay : MonoBehaviour
{
    private PlayerStatusControl psCon;  //�X�e�U���
    private Text text;  //�X�e�U��{�^���ɕ\������e�L�X�g
    [SerializeField]
    [Header("�㏸����X�e�[�^�X����� : �U���� HP �X�^�~�i")]
    private string status;
    private void Start()
    {
        psCon = PlayerStatusControl.instance;
        text = GetComponent<Text>();
        GetInfo();
    }

    private void GetInfo()
    {
        if (status == "�U����")
        {
            text.text = $"{Mathf.Abs(psCon.GetCost("atk")).ToString("F0")} exp �������\n" +
                $"{status} �� {psCon.GetGain("atk").ToString("F0")} �����㏸";
        }
        else if(status == "HP")
        {
            text.text = $"{Mathf.Abs(psCon.GetCost("hp")).ToString("F0")} exp �������\n" +
                $"{status} �� {psCon.GetGain("hp").ToString("F0")} �����㏸";
        }
        else if(status == "�X�^�~�i")
        {
            text.text = $"{Mathf.Abs(psCon.GetCost("st")).ToString("F0")} exp �������\n" +
                $"{status} �� {psCon.GetGain("st").ToString("F1")} �����㏸";
        }

    }

}
