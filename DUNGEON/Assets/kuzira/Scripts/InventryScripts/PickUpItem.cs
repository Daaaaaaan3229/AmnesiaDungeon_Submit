using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PickUpItem : MonoBehaviour
{
    //Item�f�[�^������
    [SerializeField]
    [Header("Item��ScriptableObject")]
    private Item item;

    private PlayerStatusControl psCon;  //PlayerStatusControl�̃C���X�^���X���Q�Ƃ���
    private GoldExpDisplay geDis;       //GoldExpDisplay�̃C���X�^���X���Q�Ƃ���

    private const int MAX_ITEM = 16;

    private float itemGold;//�A�C�e���̋��z

    void Start()
    {
        //�ݒ肵���A�C�R����\��������
        GetComponent<Image>().sprite = item.GetIcon();

        //�A�C�e���̋��z�̎擾
        itemGold = item.GetGold();

        //�v���C���[�X�e�[�^�X�̃C���X�^���X���擾
        psCon = PlayerStatusControl.instance;

        geDis = GoldExpDisplay.instance;
    }

    //�C���x���g���ɃA�C�e����ǉ�
    public void PickUp()
    {
        if(Inventry.instance.items.GetItems().IndexOf(null) != -1 && psCon.GetPlayerStatusSO("current").Get("gold") >= itemGold )
        {
            psCon.StatusCalculate("gold", -itemGold);
            geDis.Display();
            Inventry.instance.Add(item);
        }
    }
}
