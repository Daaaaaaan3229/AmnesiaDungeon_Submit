using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMenu : MonoBehaviour, IMenuSetting
{
    [SerializeField]
    [Header("メニューの一番上の層のオブジェクト")]
    private GameObject background;
    [SerializeField]
    [Header("インベントリーの一番上の層のオブジェクト")]
    private GameObject inventry;
    [SerializeField]
    [Header("ステータスの一番上の層のオブジェクト")]
    private GameObject statusMenu;
    private bool _isTalking;    //村人と話しているか
    private bool _displayStatus;    //Tabを押してメニューを開いているか

    public static PlayerMenu instance;
    private PlayerStatusControl psCon;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        background.SetActive(false);
        inventry.SetActive(false);
        statusMenu.SetActive(false);

        psCon = PlayerStatusControl.instance;
    }
    private void Update()
    {
        //Tabキーが押されて、プレイヤーが移動可能ならば。
        if (Input.GetKeyDown(KeyCode.Tab) && _isTalking == false)
        {
            _displayStatus = !background.activeSelf;
            DisplayBackGround(!background.activeSelf);
            PlayerMove.instance.SetMove(!background.activeSelf);
            DisplayInventry(!inventry.activeSelf);
            DisplayStatusMenu(!statusMenu.activeSelf);
            psCon.StatusUpdate();
            RegisterItemDisplay.instance.UpdateDisplayUI();
        }
    }

    private void DisplayBackGround(bool set)
    {
        background.SetActive(set);
    }
    private void DisplayInventry(bool set)
    {
        inventry.SetActive(set);
    }
    private void DisplayStatusMenu(bool set)
    {
        statusMenu.SetActive(set);
    }

    public bool GetDisplayStatus()
    {
        return _displayStatus;
    }

    public void SetTalking(bool value)
    {
        _isTalking = value;
    }
    public bool GetTalking()
    {
        return _isTalking;
    }

    /// <summary>
    /// 渡されたメニューの表示を反転させる
    /// </summary>
    /// <param name="menu">表示状態を反転させるオブジェクト</param>
    public void SetMenu(GameObject menu)
    {
        menu.SetActive(!menu.activeSelf);
        _isTalking = menu.activeSelf;
        PlayerMove.instance.SetMove(!menu.activeSelf);
    }
}
