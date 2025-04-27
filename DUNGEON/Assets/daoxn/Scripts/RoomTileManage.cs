using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTileManage : MonoBehaviour
{
    /// <summary>
    /// �q�I�u�W�F�N�g�̉摜��S�ĕ\������
    /// </summary>
    public void OnRoomImage()
    {
        foreach(Transform child in transform)
        {
            child.GetComponent<MapTileManage>().OnImage();
        }
    }
}
