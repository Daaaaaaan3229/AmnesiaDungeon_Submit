using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairManager : MonoBehaviour
{
    //�K�i�Ƀv���C���[���Ԃ��������̏���
    //UI�̕\��
    //�_���W�����}�l�[�W���[���玟�̃t���A�ɍs�������̌Ăяo��

    private DungeonManager dungeonManager;

    [Header("���̊K�ړ��I���p�l��")]
    [SerializeField]
    private GameObject nextFloorPanel;

    [Header("�J�[�\������X�N���v�g")]
    [SerializeField]
    private CursorScript cursorScript;


    private void Start()
    {
        dungeonManager = GameObject.Find("DungeonManager").GetComponent<DungeonManager>();
    }
    

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("�q�b�g");

        //���̊K�ړ��I���p�l����\��
        if (collision.gameObject.CompareTag("Player"))
        {
            nextFloorPanel.SetActive(true);

            cursorScript.OnCursor();
        }
    }

    /// <summary>
    /// ���̊K�ֈړ�����
    /// </summary>
    public void NextFloorYes()
    {
        dungeonManager.NextFloor();
    }

    /// <summary>
    /// �p�l�������
    /// </summary>
    public void NextFloorNo()
    {
        nextFloorPanel.SetActive(false);

        cursorScript.OffCursor();
    }

    /// <summary>
    /// ���ɖ߂�
    /// </summary>
    public void BactToVillage()
    {
        dungeonManager.OnBackVillage();
    }
}
