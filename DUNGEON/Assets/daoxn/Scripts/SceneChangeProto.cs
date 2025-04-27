using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//プロト版でのシーン遷移スクリプト

public class SceneChangeProto : MonoBehaviour
{
    [SerializeField]
    [Header("遷移したいシーン名")]
    private string sceneName;

    /// <summary>
    /// ボタンが押されたら、シーン遷移する
    /// </summary>
    public void OnBuuton()
    {
        SceneManager.LoadScene(sceneName);
    }
}
