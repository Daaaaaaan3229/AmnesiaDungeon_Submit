using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.UI;
using System.Linq;

public class DungeonManager : MonoBehaviour
{

    //マップの階層管理＆シーン遷移
    //モンスターの出現管理
    //dontデストロイで必要な情報があれば管理

    private int floorNum;//階層

    public static DungeonManager instance;

    [Header("階層保存のScritableObject")]
    [SerializeField]
    private FloorNumSave floorNumSave;

    [Header("敵の最大数")]
    [SerializeField]
    private int maxEnemyNum;

    [Space(10)]

    [Header("最初の敵の数最大最小")]
    [SerializeField]
    private int minStartEnemyNum;
    [SerializeField]
    private int maxStartEnemyNum;

    [Space(10)]

    [Header("敵の生成時間最大最小")]
    [SerializeField]
    private float minInstantEnemyTime;
    [SerializeField]
    private float maxInstantEnemyTime;

    [Space(10)]

    [SerializeField]
    [Header("プレイヤーオブジェクト")]
    private GameObject playerObj;


    [SerializeField]
    [Header("敵プレハブリスト")]
    List<GameObject> enemyPreHubList = new List<GameObject>();

    [SerializeField]
    List<GameObject> enemyList = new List<GameObject>();

    [SerializeField]
    [Header("フェードアウトオブジェクト")]
    private GameObject fadeoutObj;

    [SerializeField]
    [Header("ダンジョンのBGM")]
    private AudioClip[] bgms;

    private int[,] map;//マップの二次元配列。

    private float elapsedTime;//経過時間、つまり、タイマー
    private float instatTime;//生成する時間

    private bool isInstance;//敵を生成するか否か true:生成する false:生成しない  ダンジョンシーンのみで生成するため

    [SerializeField]
    public static List<GameObject> activeEnemies = new List<GameObject>();//敵の数を管理するためのstaticなList

    private PlayerStatusControl playerStatusControl;//PlayerStatusControlコンポネント

    //----オーディオ系----
    private AudioSource endAudio;//AudioSourceコンポネント

    private float fadeDuration = 3.0f;  // フェードアウトにかける時間（秒）

    private bool isFading = false; // フェード中かどうかを判定
    private float fadeStartTime;
    private float startVolume;//最初のボリュームを格納

    //-------------------

    void Awake()
    {
        //ダンジョンであれば、ダンジョン生成の方でtrueになる
        isInstance = false;

        floorNum = floorNumSave.GetCurrentFloorNum();

        SetDungeonBGM();
    }

    private void Start()
    {
        playerStatusControl = playerObj.GetComponent<PlayerStatusControl>();
    }

    private void Update()
    {
        //敵を一定時間で生成する
        if (isInstance)//isInstanceがtrueのときのみ生成する
        {
            //タイマー
            elapsedTime += Time.deltaTime;

            //生成時間をタイマーが越えたら生成する
            if(elapsedTime > instatTime)
            {
                //敵の最大数よりアクティブな敵の数が少なければ、敵を生成する
                if (maxEnemyNum < activeEnemies.Count)
                {
                    InstantEnemy();//敵を生成
                }

                //タイマーのリセット
                elapsedTime = 0;

                //次に敵を生成する時間を決める
                instatTime = Random.Range(minInstantEnemyTime, maxInstantEnemyTime);
                
            }

        }

        if (isFading)
        {
            float elapsedAudioTime = Time.time - fadeStartTime;

            if (elapsedAudioTime < fadeDuration)
            {
                endAudio.volume = Mathf.Lerp(startVolume, 0, elapsedAudioTime / fadeDuration);
            }
            else
            {
                endAudio.volume = 0; // 最終的に音量を0にする
                endAudio.Stop();    // 再生を停止
                isFading = false;
            }
        }
    }

    /// <summary>
    /// 次の階へ移動する
    /// </summary>
    public void NextFloor()
    {
        floorNum += 1;

        //ラスボス撃破後はエンディングシーンへ
        if (floorNum + 1 == 31)
        {
            SceneManager.LoadScene("EndingScene");

            return;
        }

        //フロア数の保存
        floorNumSave.FloorNumSet(floorNum);

        //リストをクリア
        activeEnemies.Clear();

        //時間での生成もリセット
        isInstance = false;

        if (floorNum > floorNumSave.GetMaxFloorNum() - 1)
        {
            SceneManager.LoadScene("Scenario");

            return;
        }

        if (floorNum + 1 == 10)
        {
            //10階ボスシーン
            SceneManager.LoadScene("BossScene10Proto");
        }
        else if(floorNum + 1 == 20)
        {
            //20階ボスシーン
            SceneManager.LoadScene("BossScene20Proto");
        }
        else if (floorNum + 1 == 30)
        {
            //ラスボスシーン
        SceneManager.LoadScene("LastBossScene");
        }
        else
        {
            //次のダンジョンフロア
            SceneManager.LoadScene("DungeonProtoScene");
        }
    }

    /// <summary>
    /// MiniMapManagerからマップの配列を取得する
    /// </summary>
    /// <param name="getMapArray"></param>
    public void StartInstantEnemy(int[,] getMapArray, GameObject getPlayerObj)
    {
        //フロア数の表示
        GameObject.Find("FloorNum").GetComponent<Text>().text = (floorNum + 1).ToString();

        //Proto版でとりあえず実装
        //プレイヤーを取得
        playerObj = getPlayerObj;
        playerStatusControl = playerObj.GetComponent<PlayerStatusControl>();

        //ボスフロアであれば敵は生成しない
        if (floorNum + 1 % 10 != 0)
        {
            map = getMapArray;

            //ランダムに開始の敵の数を決定
            int startEnemyNum = Random.Range(minStartEnemyNum, maxStartEnemyNum);

            //どの敵を生成するかを決定する
            enemyList = enemyPreHubList.OrderBy(x => UnityEngine.Random.value).Take(3).ToList();

            for (int i = 0; i < startEnemyNum; i++)
            {
                //敵の生成
                InstantEnemy();
            }
        }

        //敵的に敵を生成させる
        isInstance = true;

        //次に敵を生成する時間を決める
        instatTime = Random.Range(minInstantEnemyTime, maxInstantEnemyTime);
    }

    /// <summary>
    /// 敵を部屋に生成
    /// </summary>
    private void InstantEnemy()
    {
        int x;
        int z;

        //ランダムに座標を取得し、部屋かどうかを判別する
        do
        {
            x = Random.Range(0, map.GetLength(0));
            z = Random.Range(0, map.GetLength(1));
        }
        while (map[z, x] != 2);

        //敵を生成する
        //どの敵を生成するかはリストからランダム
        GameObject instantObj = Instantiate(enemyList[Random.Range(0, enemyList.Count)], new Vector3(x, 0.1f, z), Quaternion.identity);


        //リストに生成した敵オブジェクトを格納
        activeEnemies.Add(instantObj);

        //プレイヤーオブジェクトを渡す
        //階層を渡す
        instantObj.GetComponent<EnemyController>().SetParameter(playerObj, floorNum);

        Debug.Log("敵生成 :" + x + ", " + z + "| map[x, z] = " + map[z, x]);

    }

    /// <summary>
    /// 階段のパネルから呼び出される村に戻る処理
    /// </summary>
    public void OnBackVillage()
    {
        //村に戻る
        BackVilalge();
    }

    /// <summary>
    /// プレイヤーがHP0になった時の処理
    /// </summary>
    public void PlayerDie()
    {
        //フェードアウトをオンにする
        fadeoutObj.SetActive(true);



    }

    /// <summary>
    /// フェードアウトアニメーションから呼び出す
    /// DieSceneに遷移する
    /// </summary>
    public void ChangeDieScene()
    {
        //階層のリセット
        floorNum = 0;

        //フロア数の保存
        floorNumSave.FloorNumSet(floorNum);

        //村に戻る前に減少したステータスのリセット
        playerStatusControl.StatusSet();

        SceneManager.LoadScene("DieScene");
    }

    /// <summary>
    /// 村に戻る処理
    /// </summary>
    private void BackVilalge()
    {
        //階層のリセット
        floorNum = 0;

        //フロア数の保存
        floorNumSave.FloorNumSet(floorNum);

        //村に戻る前に減少したステータスのリセット
        playerStatusControl.StatusSet();

        //村に戻る
        SceneManager.LoadScene("VillageScene");
    }


    /// <summary>
    /// ボス撃破時の処理
    /// </summary>
    public void BossDie()
    {

        //階層のリセット
        floorNum = 0;

        //フロア数の保存
        floorNumSave.FloorNumSet(floorNum);

        //プロト版スタート画面に戻る
        SceneManager.LoadScene("StartProtoScene");
    }

    /// <summary>
    /// BGMを変更
    /// </summary>
    private void SetDungeonBGM()
    {
        //階層を5で割った値を、配列のインデックスとする
        int texIndex = floorNum / 5;

        Debug.Log("BGM配列番号 : " + texIndex);

        //ボスシーンでは、BGM1つだけ設定しておく
        //ボスシーンに該当する階層では、texIndexを0にする
        if (floorNum == 9 || floorNum == 19 || floorNum == 29)
        {
            texIndex = 0;
        }


        if (texIndex >= 6)
        {
            texIndex = 5;
        }

        //オーディオソースの取得
        //Startとかだと処理が間に合わないのでここに入れとけば安心
        endAudio = GetComponent<AudioSource>();

        //オーディオクリップの変更
        endAudio.clip = bgms[texIndex];

        //BGMの再生
        endAudio.Play();
    }

    /// <summary>
    /// BGMのボリュームを少しずつ下げて消す
    /// </summary>
    public void VolumeFadeOut()
    {
        if (!isFading)
        {
            //isFadinをtrueにするとUpdateの方で処理される
            isFading = true;
            fadeStartTime = Time.time;
            startVolume = endAudio.volume;
        }
    }

}
