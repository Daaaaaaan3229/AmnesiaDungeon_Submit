using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapTileManage : MonoBehaviour
{

    private Image tileImg;//Image�R���|�l���g

    private void Start()
    {
        //�R���|�l���g�̎擾
        tileImg = GetComponent<Image>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        //�v���C���[�|�C���g�̔���
        if (other.CompareTag("PlayerPoint"))
        {
            //�e�I�u�W�F�N�g�̖��O�ŕ����ł��邩�𔻕�
            if(transform.parent.name == "RoomTile(Clone)")
            {
                //�e�I�u�W�F�N�g������A�����̉摜��S�ĕ\��
                transform.parent.GetComponent<RoomTileManage>().OnRoomImage();
            }
            else
            {
                OnImage();
            }
        }
    }

    /// <summary>
    /// �摜��\������
    /// </summary>
    public void OnImage()
    {
        tileImg.enabled = true;
    }
}
