using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ShopScript : MonoBehaviour
{
    [SerializeField]
    [Header("表示させたいメニュー")]
    private GameObject[] menus;

    [SerializeField]
    [Header("メニューを表示可能領域に入ったことを示すテキスト")]
    private GameObject text;

    [SerializeField]
    [Header("離れる際のキー")]
    private GameObject text_leave;

    private bool _display; //TRUE：メニュー表示可能　FALSE：メニュー表示不可

    private bool isSpeech;//会話する前と後の判定 TRUE：会話後  FLASE：会話前

    private bool isHiddenDisplay;//ディスプレイを表示を消すか否か TRUE：消せる FALSE：触れない

    private IMenuSetting imenu;
    private void Awake()
    {
        text.SetActive(false);
        text_leave.SetActive(false);
        foreach(GameObject menu in menus)
        {
            menu.SetActive(false);
        }
        _display = false;
    }

    private void Update()
    {
        if (_display)
        {
            if(Input.GetKeyDown(KeyCode.F) && PlayerMenu.instance.GetDisplayStatus() == false)
            {
                PlayerMenu.instance.SetTalking(true);
                PlayerMove.instance.SetMove(false);
                //会話を表示
                if (!isSpeech)
                {
                    GetComponent<Speech>().StartSpeech();
                    isSpeech = true;

                    //ボタン非表示
                    text.SetActive(!text.activeSelf);
                }
                
                //ディスプレイを非表示にする
                if(isHiddenDisplay)
                {
                    if (menus.Count() != 0) DisplayMenu();
                    else if (menus.Count() == 0)
                    {
                        PlayerMove.instance.SetMove(true);
                        PlayerMenu.instance.SetTalking(false);
                        text.SetActive(true);
                        text_leave.SetActive(false);
                    }
                    //boolの初期化、次のために
                    isSpeech = false;
                    isHiddenDisplay = false;

                    //ボタン表示
                    if(menus.Count() != 0)text.SetActive(!text.activeSelf);
                }
            }
        }
    }

    /// <summary>
    /// 接触してきた相手に対して表示させたいメニューを渡す
    /// </summary>
    /// <param name="col"></param>
    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.GetComponent<IMenuSetting>() != null)
        {
            imenu = col.gameObject.GetComponent<IMenuSetting>();
            text.SetActive(true);
            _display = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.GetComponent<IMenuSetting>() != null)
        {
            _display = false;
            text.SetActive(false);
        }
    }

    /// <summary>
    /// Speech.csから、DisplayMenu()を呼び出す
    /// </summary>
    public void OnDisplayMenu()
    {
        //ディスプレイの表示
        if(menus.Count() != 0)DisplayMenu();
        else if(menus.Count() == 0)text_leave.SetActive(true);
        //ディスプレイを非表示にできるようにする
        isHiddenDisplay = true;
    }


    public void DisplayMenu()
    {
        if(imenu != null)
        {
            foreach (GameObject menu in menus)
            {
                imenu.SetMenu(menu);
            }
            GoldExpDisplay.instance.Display();
        }
    }
}
