﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;

public class ScenarioManager : MonoBehaviour
{
    [Header("階層保存のScritableObject")]
    [SerializeField]
    private FloorNumSave floorNumSave;

    // 対象のテキスト
    [SerializeField] private TMP_Text _text;

    // 次の文字を表示するまでの時間[s]
    [SerializeField] private float _delayDuration = 0.1f;

    [Header("シナリオのCSVファイル")]
    [SerializeField] private List<TextAsset> csvScenarioFiles = new List<TextAsset>();

    //取得したCSVファイルの中身をここに格納する
    private List<string[]> csvDatas = new List<string[]>();

    private Coroutine _showCoroutine;

    [SerializeField]
    private int floorNum = 0;//ダンジョンの階層

    private int skipShow;//表示を早くするかを数字で状態切り替え 0:速度切り替えできない 1:通常速度 2:速度切り替え処理をする 3:速度UP

    private bool onNextScene;//次のシーンに移動できるか否か true:移動できる false:移動できない

    private void Start()
    {
        floorNum = floorNumSave.GetCurrentFloorNum();

        ReadCSV();

        Show();
    }

    private void Update()
    {
        //画面左クリックで表示を早くする
        if (Input.GetMouseButtonDown(0) && skipShow == 1)
        {
            _delayDuration = 0.01f;

            skipShow = 2;
        }

        //画面クリックで次のシーンへ進む
        if (onNextScene)
        {
            if (Input.GetMouseButtonDown(0))
            {
                NextScene();
            }
        }
    }

    /// <summary>
    /// CSVファイルの読み込み
    /// </summary>
    private void ReadCSV()
    {
        StringReader reader = new StringReader(csvScenarioFiles[floorNum].text);


        while (reader.Peek() != -1) // reader.Peaekが-1になるまで
        {
            string line = reader.ReadLine(); // 一行ずつ読み込み
            csvDatas.Add(line.Split(',')); // , 区切りでリストに追加
        }
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

        //シナリオを合体する
        string sumText = "";

        for(int i = 1; i < csvDatas.Count; i++)
        {
            sumText += csvDatas[i][1] + "\n";
        }

        //合体したテキストをUIに格納
        _text.text = sumText;

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

        //for(int h = 0; h < )

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

        onNextScene = true;

        _showCoroutine = null;
    }

    private void NextScene()
    {

        if (floorNum + 1 == 10)
        {
            //10階ボスシーン
            SceneManager.LoadScene("BossScene10Proto");
        }
        else if (floorNum + 1 == 20)
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


}
