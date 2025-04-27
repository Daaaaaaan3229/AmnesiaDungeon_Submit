using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairManager : MonoBehaviour
{
    //階段にプレイヤーがぶつかった時の処理
    //UIの表示
    //ダンジョンマネージャーから次のフロアに行く処理の呼び出し

    private DungeonManager dungeonManager;

    [Header("次の階移動選択パネル")]
    [SerializeField]
    private GameObject nextFloorPanel;

    [Header("カーソル制御スクリプト")]
    [SerializeField]
    private CursorScript cursorScript;


    private void Start()
    {
        dungeonManager = GameObject.Find("DungeonManager").GetComponent<DungeonManager>();
    }
    

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("ヒット");

        //次の階移動選択パネルを表示
        if (collision.gameObject.CompareTag("Player"))
        {
            nextFloorPanel.SetActive(true);

            cursorScript.OnCursor();
        }
    }

    /// <summary>
    /// 次の階へ移動する
    /// </summary>
    public void NextFloorYes()
    {
        dungeonManager.NextFloor();
    }

    /// <summary>
    /// パネルを閉じる
    /// </summary>
    public void NextFloorNo()
    {
        nextFloorPanel.SetActive(false);

        cursorScript.OffCursor();
    }

    /// <summary>
    /// 村に戻る
    /// </summary>
    public void BactToVillage()
    {
        dungeonManager.OnBackVillage();
    }
}
