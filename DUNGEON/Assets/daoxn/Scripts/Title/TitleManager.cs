using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [Header("タイトル画像")]
    [SerializeField]
    private GameObject titleImage;

    [Header("スタートテキスト")]
    [SerializeField]
    private GameObject startText;

    private bool onStartGame;//ゲームをスタートできるか否か true:スタートできる false:スタートできない

    [Header("TowerMoveがアタッチされたオブジェクトリスト")]
    [SerializeField]
    private List<TowerMove> towerMoves = new List<TowerMove>();

    AudioSource audioSource;//AudioSourceコンポネント

    [Header("風効果音")]
    [SerializeField]
    AudioClip wind;

    [Header("BGM")]
    [SerializeField]
    AudioClip bgm;

    public float fadeDuration = 3f;  // フェードアウトにかかる時間


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.PlayOneShot(wind);
    }
    private void Update()
    {
        if (onStartGame)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene("IntoroScene");
            }
        }
    }

    /// <summary>
    /// 音を徐々に小さくする
    /// </summary>
    public void OnFadeOutWind()
    {
        float startVolume = audioSource.volume;

    }

    /// <summary>
    /// タイトルBGMを流す
    /// </summary>
    public void OnTitleBGM()
    {

        audioSource.PlayOneShot(bgm);           // BGMを再生
    }

    /// <summary>
    /// タイトル画像をオンにする。Animationイベントから呼び出す
    /// </summary>
    public void OnTitle()
    {
        titleImage.SetActive(true);

        //全てのTowerMoveをオンにする
        foreach (TowerMove x in towerMoves)
        {
            x.enabled = true;
        }

        audioSource.clip = bgm;  // AudioSource にBGMを設定
        audioSource.loop = true;      // BGMがループするように設定
    }

    /// <summary>
    /// ゲームをスタートできるようにする
    /// </summary>
    public void OnAbeleStart()
    {
        onStartGame = true;

        startText.SetActive(true);
    }
}
