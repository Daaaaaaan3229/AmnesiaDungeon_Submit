using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndStorySenarioManager : StorySenarioManager
{
    private AudioSource endAudio;//BGMのオーディオソース

    [Header("タイトル画像")]
    [SerializeField]private GameObject endTitleImage;

    [Header("クレジットテキスト")]
    [SerializeField] private GameObject creditText;

    private bool onEnd;//エンドクレジットが終わったか否か true : 終わった  false : 終わってない

    //----オーディオ系----
    private float fadeDuration = 3.0f;  // フェードアウトにかける時間（秒）

    private bool isFading = false; // フェード中かどうかを判定
    private float fadeStartTime;
    private float startVolume;//最初のボリュームを格納

    //-------------------

    public override void Update()
    {
        base.Update();

        //ボリュームを下げる
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

    public override void Show()
    {
        //エンディングシーンが終わっていたらタイトルに戻る
        if (onEnd)
        {
            NextScene();

            return;
        }

        //エンディングシーンでは、文字送りが終わったら別な演出のメソッドを呼び出す
        if (speechNum >= csvDatas.Count)
        {
            EndCreditStart();

            return;
        }

        base.Show();
    }

    /// <summary>
    /// エンドクレジットの演出スタート
    /// </summary>
    private void EndCreditStart()
    {
        //テキストをオフに
        _text.enabled = false;

        endAudio = GetComponent<AudioSource>();

        //BGMオン
        endAudio.Play();

        //タイトル画像アニメーションをオン
        endTitleImage.SetActive(true);

        //イベントでクレジットアニメーションをオン

        //イベントで効果音を小さく
    }

    /// <summary>
    /// クレジットオブジェクトをオンにする
    /// </summary>
    public void OnCredit()
    {
        creditText.SetActive(true);
    }

    /// <summary>
    /// エンドクレジットが終わって、シーン移動できるようにする
    /// </summary>
    public void FinishEndCredit()
    {
        VolumeFadeOut();

        onEnd = true;

        nextTextStay = true;
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
