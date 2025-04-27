using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleAnimEventCamera : MonoBehaviour
{
    [SerializeField]
    private TitleManager titleManager;//TitleManagerコンポネント。イベントトリガーから関数を呼び出す

    public void OnAnimTitleImage()
    {
        titleManager.OnTitle();
    }

    public void OnAnimStartText()
    {
        titleManager.OnAbeleStart();
    }

    public void StopWind()
    {
        titleManager.OnFadeOutWind();
    }

    public void StartBGM()
    {
        titleManager.OnTitleBGM();
    }
}
