using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapManage : MonoBehaviour
{
    private int[,] map;//マップの二次元配列。マップタイルの属性を判定するのに使う。

    [SerializeField]
    [Header("ミニマップを格納する親オブジェクト")]
    private GameObject mapCanvasObj;

    [SerializeField]
    [Header("ミニマップタイルのプレハブ")]
    private GameObject mapTilePrefab;

    [SerializeField]
    [Header("部屋を格納する親のプレハブ")]
    private GameObject roomParentPrefab;

    [SerializeField]
    [Header("プレイヤーオブジェクト")]
    private GameObject playerObject;

    [SerializeField]
    [Header("マップ上のプレイヤーの位置")]
    private GameObject playerPoint;

    private Vector3 playerPos;//プレイヤーの現在地

    private RectTransform playerPointPos;//プレイヤーポイントの位置

    [SerializeField]
    [Header("マップ上の階段の位置")]
    private GameObject stairPoint;

    [SerializeField]
    [Header("ミニマップ位置調整数値")]
    private int slideValueX;
    [SerializeField]
    private int slideValueY;


    private List<Vector2> roomEnd = new List<Vector2>();//各部屋の端の座標を保存

    private List<GameObject> roomParentList = new List<GameObject>();//部屋の親のリスト

    private bool checkRoom;//前の座標が部屋かどうかを判定 true:部屋 false:部屋じゃない

    private int[] previousRoomNum;//前の数値を格納する配列

    private int roomParentNum;//格納する部屋の番号

    private Canvas miniMapCanvas;//このオブジェクトのCanvasコンポネント

    private void Start()
    {
        playerPointPos = playerPoint.GetComponent<RectTransform>();

        miniMapCanvas = this.gameObject.GetComponent<Canvas>();
    }

    private void Update()
    {
        //プレイヤーの現在地を取得し、UI座標に調整
        playerPos = new Vector3((-playerObject.transform.position.z + slideValueX) * 10, (playerObject.transform.position.x - slideValueY) *10, 0);

        //プレイヤーポイントをプレイヤーの位置に
        playerPointPos.localPosition = playerPos;

        if (Input.GetKeyDown(KeyCode.M))
        {
            miniMapCanvas.enabled = !miniMapCanvas.enabled;
        }
    }

    public void MiniMapGenerate(int[,] getMapArray)
    {
        //二次配列の初期化
        map = new int[getMapArray.GetLength(0), getMapArray.GetLength(1)];

        previousRoomNum = new int[getMapArray.GetLength(1)];

        // 配列の仕分け
        // 壁:0 道:1 部屋:2～
        for (int i = 0; i < getMapArray.GetLength(0); i++) // 行のループ
        {
            for (int j = 0; j < getMapArray.GetLength(1); j++) // 列のループ
            {
                Debug.Log("Value at [" + i + "," + j + "] is: " + getMapArray[i, j]);

                //取得した配列を格納。
                //room = 2  road = 3
                map[i, j] = getMapArray[i, j];

                //ミニマップの座標を格納
                Vector2 minimapPos = new Vector2(-i, j);

                // マップの属性が壁でなければタイルを生成する
                if(map[i,j] == 2)
                {
                    if (!checkRoom)
                    {
                        checkRoom = true;

                        if(!roomEnd.Contains(minimapPos))
                        {
                            //部屋の端を保存
                            roomEnd.Add(minimapPos);

                            //部屋の親を新しく作成
                            GameObject roomParent = Instantiate(roomParentPrefab, Vector3.zero, Quaternion.identity);

                            //親オブジェクトの設定
                            roomParent.transform.SetParent(mapCanvasObj.transform, false);

                            //部屋の親リストに追加
                            roomParentList.Add(roomParent);
                        }

                        //リストの何番目にあるかを取得
                        roomParentNum = roomEnd.IndexOf(minimapPos);
                    }

                    previousRoomNum[j] = map[i, j];

                    map[i, j] = roomParentNum + 2;

                    //道タイルの検索と追加
                    //部屋から右の道チェック
                    if(map[i -1, j] == 1)
                    {
                        map[i - 1, j] = roomParentNum + 2;
                    }

                    //部屋から下の道チェック
                    if (map[i, j - 1] == 1)
                    {
                        map[i, j - 1] = roomParentNum + 2;
                    }
                }
                else
                {
                    //用済みの部屋の端の座標を別なとこにずらしておく
                    if (roomEnd.Contains(minimapPos))
                    {
                        roomEnd[roomEnd.IndexOf(minimapPos)] = new Vector2(10, 10);
                    }

                    if (map[i, j] == 3)
                    {
                        previousRoomNum[j] = map[i, j];

                        map[i, j] = 1;

                        checkRoom = false;

                        //道タイルの検索と追加
                        //部屋から上の道チェック
                        if (map[i, j - 1] >= 2 && (map[i -1, j - 1] >= 2 || map[i + 1, j -1] >= 2))
                        {
                            map[i, j] = map[i, j - 1];
                        }

                        //部屋から左の道チェック
                        if (map[i - 1, j] >= 2 && (map[i - 1, j - 1] >= 2 || map[i - 1, j + 1] >= 2))
                        {
                            map[i, j] = map[i - 1, j];
                        }
                    }
                    else
                    {
                        previousRoomNum[j] = map[i, j];

                        map[i, j] = 0;

                        checkRoom = false;
                    }
                }

            }

            for (int k = 0; k < roomEnd.Count; k++)
            {
                roomEnd[k] += new Vector2(-1, 0);
            }
        }

        //タイルの生成
        for (int i = 0; i < getMapArray.GetLength(0); i++) // 行のループ
        {
            for (int j = 0; j < getMapArray.GetLength(1); j++) // 列のループ
            {
                if (map[i, j] != 0)
                {
                    //タイルの生成
                    //slideValueでマップの位置を調整
                    GameObject mapTileObj = Instantiate(mapTilePrefab, new Vector3((-i + slideValueX) * 10, (j - slideValueY) * 10, 0), Quaternion.identity);

                    if (map[i, j] == 1)
                    {
                        //親オブジェクトをMiniMapCanvasに設定
                        mapTileObj.transform.SetParent(mapCanvasObj.transform, false);
                    }
                    else
                    {
                        //親オブジェクトをRoomTileに設定
                        mapTileObj.transform.SetParent(roomParentList[map[i, j] - 2].transform, false);
                    }
                }
            }
        }
    }

    public void StairOnMiniMap(int stairX, int stairZ)
    {
        //階段をミニマップに配置
        stairPoint.transform.localPosition = new Vector3((-stairZ + slideValueX) * 10, (stairX - slideValueY) * 10, 0); ;

        //階段に親オブジェクトを設定
        stairPoint.transform.SetParent(roomParentList[map[stairZ, stairX] - 2].transform, false);

        Debug.Log("取得座標 > stairX ;" + stairX + "stairZ :" + stairZ + "/n マップ属性 :" +  map[stairX, stairZ]);

    }
}
