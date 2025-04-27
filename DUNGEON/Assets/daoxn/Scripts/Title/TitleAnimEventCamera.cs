using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleAnimEventCamera : MonoBehaviour
{
    [SerializeField]
    private TitleManager titleManager;//TitleManager�R���|�l���g�B�C�x���g�g���K�[����֐����Ăяo��

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
