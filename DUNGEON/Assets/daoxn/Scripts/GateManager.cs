using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GateManager : MonoBehaviour
{
    //村からダンジョンへ移動する

    [Header("ダンジョン挑戦しますかパネル")]
    [SerializeField]
    private GameObject nextFloorPanel;

    [Header("カーソル制御スクリプト")]
    [SerializeField]
    private CursorScript cursorScript;

    [Header("階層保存のScritableObject")]
    [SerializeField]
    private FloorNumSave floorNumSave;


    private void OnCollisionEnter(Collision collision)
    {

        //ダンジョン移動選択パネルを表示
        if (collision.gameObject.CompareTag("Player"))
        {
            nextFloorPanel.SetActive(true);

            cursorScript.OnCursor();
        }
    }

    /// <summary>
    /// ダンジョンへ移動する
    /// </summary>
    public void GoDungeonYes()
    {

        if(floorNumSave.GetMaxFloorNum() == 0)
        {
            //シナリオシーン
            SceneManager.LoadScene("Scenario");
        }
        else
        {
            SceneManager.LoadScene("DungeonProtoScene");
        }
        
    }

    /// <summary>
    /// パネルを閉じる
    /// </summary>
    public void GoDungeonNo()
    {
        nextFloorPanel.SetActive(false);

        cursorScript.OffCursor();
    }
}
