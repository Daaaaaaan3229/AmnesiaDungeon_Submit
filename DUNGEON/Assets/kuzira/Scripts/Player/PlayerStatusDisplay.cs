using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusDisplay : MonoBehaviour
{
    [Header("�X�e�[�^�X�\���p�̃e�L�X�g")]
    [SerializeField]
    private Text status;

    private PlayerStatusControl statusControl;

    private void Start()
    {
        statusControl = GetComponent<PlayerStatusControl>();
        StatusUpdate();
    }

    /// <summary>
    /// �X�e�[�^�X�\���̍X�V
    /// </summary>
    public void StatusUpdate()
    {
        status.text = $"�̗� : {statusControl.GetPlayerStatusSO("current").Get("hp").ToString("F0")} / {statusControl.GetPlayerStatusSO("max").Get("hp").ToString("F0")}\n" +
            $"�X�^�~�i : {statusControl.GetPlayerStatusSO("current").Get("st").ToString("F0")} / {statusControl.GetPlayerStatusSO("max").Get("st").ToString("F0")}\n" +
            $"�U���� : {statusControl.GetPlayerStatusSO("current").Get("atk").ToString("F0")} / {statusControl.GetPlayerStatusSO("max").Get("atk").ToString("F0")}\n" + 
            $"���� : {statusControl.GetPlayerStatusSO("current").Get("gold").ToString("F0")}\n" +
            $"�o���l : {statusControl.GetPlayerStatusSO("current").Get("exp").ToString("F0")}";
    }
}
