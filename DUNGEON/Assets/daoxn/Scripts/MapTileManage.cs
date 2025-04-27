using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapTileManage : MonoBehaviour
{

    private Image tileImg;//Imageコンポネント

    private void Start()
    {
        //コンポネントの取得
        tileImg = GetComponent<Image>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        //プレイヤーポイントの判別
        if (other.CompareTag("PlayerPoint"))
        {
            //親オブジェクトの名前で部屋であるかを判別
            if(transform.parent.name == "RoomTile(Clone)")
            {
                //親オブジェクト側から、部屋の画像を全て表示
                transform.parent.GetComponent<RoomTileManage>().OnRoomImage();
            }
            else
            {
                OnImage();
            }
        }
    }

    /// <summary>
    /// 画像を表示する
    /// </summary>
    public void OnImage()
    {
        tileImg.enabled = true;
    }
}
