using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;

public class StorySenarioManager : MonoBehaviour
{
    // 対象のテキスト
    [SerializeField] protected TMP_Text _text;

    // 次の文字を表示するまでの時間[s]
    [SerializeField] private float _delayDuration = 0.1f;

    [Header("シナリオのCSVファイル")]
    [SerializeField] private TextAsset csvScenarioFile;

    [Header("次のシーン名")]
    [SerializeField] private string nextSceneName;

    //取得したCSVファイルの中身をここに格納する
    protected List<string> csvDatas = new List<string>();

    private Coroutine _showCoroutine;

    private int skipShow;//表示を早くするかを数字で状態切り替え 0:速度切り替えできない 1:通常速度 2:速度切り替え処理をする 3:速度UP

    protected int speechNum = 1;//どのセリフを表示するか

    protected bool nextTextStay;//次のセリフを表示できる状態か否か true:表示待機 false:表示できない


    private bool isFinishSpeech;//最後のセリフが終わったか否か true:終わった false:終わってない


    private void Start()
    {
        //初期化
        speechNum = 1;
        nextTextStay = false;

        ReadCSV();
    }

    public virtual void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //画面左クリックで表示を早くする
            if (skipShow == 1)
            {
                _delayDuration = 0.01f;

                skipShow = 2;
            }

            //次のセリフを表示する
            if (nextTextStay)
            {
                speechNum++;

                nextTextStay = false;

                Show();
            }
        }

    }

    /// <summary>
    /// CSVファイルの読み込み
    /// </summary>
    private void ReadCSV()
    {
        StringReader reader = new StringReader(csvScenarioFile.text);


        while (reader.Peek() != -1) // reader.Peaekが-1になるまで
        {
            string line = reader.ReadLine(); // 一行ずつ読み込み

            //string cleanedLine = line.Replace("\"", "").Trim();// ダブルクオーテーションを取り除き、空白行をスキップ

            string formattedLine = line.Replace("/n", "\n");

            csvDatas.Add(formattedLine); //リストに追加
        }

        Show();
    }

    /// <summary>
    /// 文字送り演出を表示する
    /// </summary>
    public virtual void Show()
    {
        if (speechNum >= csvDatas.Count)
        {
            NextScene();

            return;
        }

        // 前回の演出処理が走っていたら、停止
        if (_showCoroutine != null)
        {
            StopCoroutine(_showCoroutine);
        }

        //指定したテキストをUIに格納
        _text.text = csvDatas[speechNum];

        //速度切り替えを可能にする
        skipShow = 1;

        // １文字ずつ表示する演出のコルーチンを実行する
        _showCoroutine = StartCoroutine(ShowCoroutine());
    }

    /// <summary>
    /// １文字ずつ表示する演出のコルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShowCoroutine()
    {
        // 待機用コルーチン
        // GC Allocを最小化するためキャッシュしておく
        var delay = new WaitForSeconds(_delayDuration);

        // テキスト全体の長さ
        var length = _text.text.Length;

        // １文字ずつ表示する演出
        for (int i = 0; i < length; i++)
        {
            //速度を上げる
            if (skipShow == 2)
            {
                //delayの更新
                delay = new WaitForSeconds(_delayDuration);

                skipShow = 3;
            }

            // 徐々に表示文字数を増やしていく
            _text.maxVisibleCharacters = i;

            // 一定時間待機
            yield return delay;
        }

        // 演出が終わったら全ての文字を表示する
        _text.maxVisibleCharacters = length;

        //速度切り替えを戻す
        skipShow = 0;
        _delayDuration = 0.1f;

        //次のセリフを表示できるようにする
        nextTextStay = true;

        _showCoroutine = null;
    }

    public void NextScene()
    {
        //次のダンジョンフロア
        SceneManager.LoadScene(nextSceneName);
    }
}
