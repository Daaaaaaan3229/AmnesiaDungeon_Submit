using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTileManage : MonoBehaviour
{
    /// <summary>
    /// 子オブジェクトの画像を全て表示する
    /// </summary>
    public void OnRoomImage()
    {
        foreach(Transform child in transform)
        {
            child.GetComponent<MapTileManage>().OnImage();
        }
    }
}
