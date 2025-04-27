using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ShopScript : MonoBehaviour
{
    [SerializeField]
    [Header("�\�������������j���[")]
    private GameObject[] menus;

    [SerializeField]
    [Header("���j���[��\���\�̈�ɓ��������Ƃ������e�L�X�g")]
    private GameObject text;

    [SerializeField]
    [Header("�����ۂ̃L�[")]
    private GameObject text_leave;

    private bool _display; //TRUE�F���j���[�\���\�@FALSE�F���j���[�\���s��

    private bool isSpeech;//��b����O�ƌ�̔��� TRUE�F��b��  FLASE�F��b�O

    private bool isHiddenDisplay;//�f�B�X�v���C��\�����������ۂ� TRUE�F������ FALSE�F�G��Ȃ�

    private IMenuSetting imenu;
    private void Awake()
    {
        text.SetActive(false);
        text_leave.SetActive(false);
        foreach(GameObject menu in menus)
        {
            menu.SetActive(false);
        }
        _display = false;
    }

    private void Update()
    {
        if (_display)
        {
            if(Input.GetKeyDown(KeyCode.F) && PlayerMenu.instance.GetDisplayStatus() == false)
            {
                PlayerMenu.instance.SetTalking(true);
                PlayerMove.instance.SetMove(false);
                //��b��\��
                if (!isSpeech)
                {
                    GetComponent<Speech>().StartSpeech();
                    isSpeech = true;

                    //�{�^����\��
                    text.SetActive(!text.activeSelf);
                }
                
                //�f�B�X�v���C���\���ɂ���
                if(isHiddenDisplay)
                {
                    if (menus.Count() != 0) DisplayMenu();
                    else if (menus.Count() == 0)
                    {
                        PlayerMove.instance.SetMove(true);
                        PlayerMenu.instance.SetTalking(false);
                        text.SetActive(true);
                        text_leave.SetActive(false);
                    }
                    //bool�̏������A���̂��߂�
                    isSpeech = false;
                    isHiddenDisplay = false;

                    //�{�^���\��
                    if(menus.Count() != 0)text.SetActive(!text.activeSelf);
                }
            }
        }
    }

    /// <summary>
    /// �ڐG���Ă�������ɑ΂��ĕ\�������������j���[��n��
    /// </summary>
    /// <param name="col"></param>
    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.GetComponent<IMenuSetting>() != null)
        {
            imenu = col.gameObject.GetComponent<IMenuSetting>();
            text.SetActive(true);
            _display = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.GetComponent<IMenuSetting>() != null)
        {
            _display = false;
            text.SetActive(false);
        }
    }

    /// <summary>
    /// Speech.cs����ADisplayMenu()���Ăяo��
    /// </summary>
    public void OnDisplayMenu()
    {
        //�f�B�X�v���C�̕\��
        if(menus.Count() != 0)DisplayMenu();
        else if(menus.Count() == 0)text_leave.SetActive(true);
        //�f�B�X�v���C���\���ɂł���悤�ɂ���
        isHiddenDisplay = true;
    }


    public void DisplayMenu()
    {
        if(imenu != null)
        {
            foreach (GameObject menu in menus)
            {
                imenu.SetMenu(menu);
            }
            GoldExpDisplay.instance.Display();
        }
    }
}
