using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class Speech : MonoBehaviour
{
    // 対象のテキスト
    [SerializeField] private TMP_Text _text;

    // 次の文字を表示するまでの時間[s]
    [SerializeField] private float _delayDuration = 0.1f;

    [Header("セリフのCSVファイル")]
    [SerializeField] private TextAsset csvSpeechFiles;

    //取得したCSVファイルの中身をここに格納する
    private List<string[]> csvDatas = new List<string[]>();

    private Coroutine _showCoroutine;

    private int skipShow;//表示を早くするかを数字で状態切り替え 0:速度切り替えできない 1:通常速度 2:速度切り替え処理をする 3:速度UP

    private int speechNum = 1;//どのセリフを表示するか

    private bool nextTextStay;//次のセリフを表示できる状態か否か true:表示待機 false:表示できない

    private bool isEndSpeech;//最後のセリフか否か true:最後のセリフ false:最後のセリフではない

    private bool isFinishSpeech;//最後のセリフが終わったか否か true:終わった false:終わってない

    private bool isDisplayMenu;//購入UIを表示したか否か true:表示済み false:まだ表示してない

    private int maxFloorNum;//最大のダンジョン階数

    [SerializeField] int testFloorNum;//現在のダンジョンの数

    private int startLine = 0;//どこのセリフからスタートするか

    [Header("会話の最後の決まり文句")]
    [SerializeField]
    private string setPhrase;

    [Header("会話表示のテキストボックス")]
    [SerializeField]
    private GameObject textBoxObj;

    /// <summary>
    /// 会話のスタート
    /// </summary>
    public void StartSpeech()
    {
        //初期化
        speechNum = 1;
        nextTextStay = false;
        isEndSpeech = false;
        isFinishSpeech = false;
        isDisplayMenu = false;
        startLine = 0;

        //会話UIの表示
        textBoxObj.SetActive(true);

        //会話スタート
        ReadCSV();
    }

    private void Update()
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
                speechNum += 1;

                nextTextStay = false;

                Show();
            }

            //セリフの終わり
            if(isEndSpeech && isFinishSpeech && !isDisplayMenu)
            {

                //購入UIの表示
                GetComponent<ShopScript>().OnDisplayMenu();

                //会話UIの非表示
                textBoxObj.SetActive(false);

                isDisplayMenu = true;
            }

        }
    }

    /// <summary>
    /// CSVファイルの読み込み
    /// </summary>
    private void ReadCSV()
    {
        //CSVファイルの格納
        StringReader reader = new StringReader(csvSpeechFiles.text);


        while (reader.Peek() != -1) // reader.Peaekが-1になるまで
        {
            string line = reader.ReadLine(); // 一行ずつ読み込み
            csvDatas.Add(line.Split(',')); // , 区切りでリストに追加
        }

        FindNumFromCSV();
    }

    /// <summary>
    /// CSV内の階層の数字を探す
    /// </summary>
    private void FindNumFromCSV()
    {
        maxFloorNum = ConvertFloorNum(testFloorNum);

        foreach(string[] line in csvDatas)
        {
            //どこから始めるかを取得するため
            startLine += 1;

            //条件に合う行を見つけたら、そこでストップ
            if (IsNumber(line[0]))
            {
                if (maxFloorNum == int.Parse(line[0]))
                {
                    break;
                }
            }
        }

        //どこから会話をスタートするかを格納
        speechNum = startLine;

        Show();
    }

    // 行が数字で始まるかを判定するメソッド
    bool IsNumber(string line)
    {
        // 行の最初の文字が数字かどうかをチェック
        return !string.IsNullOrEmpty(line) && char.IsDigit(line[0]);
    }

    /// <summary>
    /// 文字送り演出を表示する
    /// </summary>
    public void Show()
    {
        // 前回の演出処理が走っていたら、停止
        if (_showCoroutine != null)
        {
            StopCoroutine(_showCoroutine);
        }

        //指定したテキストをUIに格納
        _text.text = csvDatas[speechNum][1];


        if(_text.text == "")
        {
            EndSpeech();

            return;
        }

        //速度切り替えを可能にする
        skipShow = 1;

        // １文字ずつ表示する演出のコルーチンを実行する
        _showCoroutine = StartCoroutine(ShowCoroutine());
    }

    // １文字ずつ表示する演出のコルーチン
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
        if (!isEndSpeech)
        {
            nextTextStay = true;
        }
        else
        {
            //会話を終わりにする
            isFinishSpeech = true;
        }

        _showCoroutine = null;
    }

    //会話の終わり
    private void EndSpeech()
    {
        // 前回の演出処理が走っていたら、停止
        if (_showCoroutine != null)
        {
            StopCoroutine(_showCoroutine);
        }

        //決まり文句テキストをUIに格納
        _text.text = setPhrase;

        //速度切り替えを可能にする
        skipShow = 1;

        //次のセリフを表示できないようにする
        isEndSpeech = true;

        // １文字ずつ表示する演出のコルーチンを実行する
        _showCoroutine = StartCoroutine(ShowCoroutine());

    }

    // 階層を5階ごとに変換する
    private int ConvertFloorNum(int num)
    {
        if (num >= 25)
        {
            return 25;
        }
        else
        {
            return (num / 5) * 5;
        }
    }

}
