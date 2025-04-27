using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    private bool _isDisplay;

    private PlayerStatusControl psCon;      //PlayerStatusControlのインスタンスの取得用
    private EffectDurationControl edCon;    //EffectDurationControlのインスタンスの取得用

    private Item item;//Itemスクリタブルオブジェクト

    private int index;
    private int rIndex;

    [SerializeField]
    [Header("使用、登録のボタンを表示させるため、二つのボタンの親オブジェクトを参照")]
    private GameObject panel;

    [SerializeField]
    [Header("何番目のスロットであるか")]
    private int slotNum;

    private void Awake()
    {
        panel.SetActive(false);
    }

    private void Start()
    {
        psCon = PlayerStatusControl.instance;
        edCon = EffectDurationControl.instance;
    }

    //アイテムを追加する
    public void AddItem(Item newItem)
    {
        //自分のスロットにどのアイテムがあるかを保存
        item = newItem;

        index = Inventry.instance.items.GetItems().IndexOf(newItem);
        GetComponent<Image>().sprite = item.GetIcon();
    }
    //アイテムを取り除く
    public void ClearItem()
    {
        item = null;
        GetComponent<Image>().sprite = null;
    }
    //アイテムの消去ボタン
    public void OnRemoveButton()
    {
        Inventry.instance.Remove(slotNum);
    }

    //アイテム使用ボタン
    //ボタンのイベントはvoidメソッドのみなので、こちらで回避
    public void UseItemButton()
    {
        UseItem();
    }

    //アイテムの使用処理
    public bool UseItem()
    {
        if (PlayerStatusControl.instance == null)
        {
            Debug.LogError("PlayerStatusControl.instance is null. PlayerStatusControl may have been destroyed.");
        }

        if(psCon == null)
        {
            psCon = PlayerStatusControl.instance;
        }

        if(edCon == null)
        {
            edCon = EffectDurationControl.instance;
        }

        //村にいるかどうかのログ
        Debug.Log(psCon.GetInVillage());

        //アイテムがなければ処理終了 = アイテムを使えない
        if (item == null)
        {
            return false;
        }
        else if(psCon.GetInVillage() == true) //村にいる場合もアイテムを使えない
        {
            return false;
        }
        PlayerStatus status = PlayerStatusControl.instance.GetPlayerStatusSO("current"); //現在のステータス情報の取得
        PlayerStatus max_status = PlayerStatusControl.instance.GetPlayerStatusSO("max"); //最大値のステータス情報の取得
        bool _use = true;
        switch (item.GetCurrentState())
        {
            case Effect_List.HP_Recover:     //体力の回復
                if (status.Get("hp") >= max_status.Get("hp")) 
                {
                    Debug.Log("体力は満タンです");
                    _use = false;
                    break;
                }
                //即時回復のみ
                Debug.Log(item.GetName() + "を使用して、" + item.GetValue() + "だけHPを回復しました！");

                //最大HPを越えないように回復する
                float newHp = Mathf.Min(status.Get("hp") + item.GetValue(), max_status.Get("hp"));

                status.Set("hp", newHp);
                break;

            case Effect_List.ST_Recover:     //スタミナの回復
                //即時回復か、持続回復かの処理
                if (item.GetTimeState() == Effect_Time.Instant && status.Get("st") != max_status.Get("st"))              //即時回復の場合
                {
                    Debug.Log(item.GetName() + "を使用して、" + item.GetValue() + "だけスタミナを回復しました！");
                    psCon.StatusCalculate("st", item.GetValue());
                    break;
                }
                else if(item.GetTimeState() == Effect_Time.Instant && status.Get("st") == max_status.Get("st"))
                {
                    _use = false;
                    break;
                }
                else if (item.GetTimeState() == Effect_Time.Continuation)    //持続効果の場合
                {
                    Debug.Log(item.GetName() + "を使用して、スタミナの上限を" + item.GetTime() + "秒間、" + item.GetValue() + "倍にします。");
                    edCon.SetTime("st", item.GetTime());   //効果時間の設定
                    psCon.SetAmpStamina(item.GetValue());   //効果量の設定
                    edCon.SetBool("st");                    //効果時間の測定開始
                    break;
                }
                break;

            case Effect_List.MAG_Recover:    //魔力の回復(未実装)
                Debug.Log(item.GetName() + "を使用して、" + item.GetValue() + "だけ魔力を回復しました！");
                break;

            case Effect_List.ATK_UP:
                if (item.GetTimeState() == Effect_Time.Continuation)    //持続効果の場合
                {
                    Debug.Log(item.GetName() + "を使用して、攻撃力を" + item.GetTime() + "秒間だけ" + item.GetValue() + "倍にします。");
                    edCon.SetBool("atk");                   //効果時間の測定開始
                    psCon.SetAmpAtkAll(item.GetValue());    //攻撃力の上昇率を設定
                    edCon.SetTime("atk", item.GetTime());   //効果時間を設定
                    psCon.SetPlayerAtk();                   //攻撃力を各コライダーに設定
                    break;
                }
                else if(item.GetTimeState() == Effect_Time.Next)        //次の攻撃時のみ効果を発揮する場合
                {
                    Debug.Log(item.GetName() + "を使用して、次回攻撃時に" + item.GetValue() + "ダメージを与えます。");
                    psCon.SetNextUp(true);                  //次回攻撃時ダメージを定数にするためのBool設定。
                    psCon.SetNextAtk(item.GetValue());      //次回攻撃時のダメージを設定。
                    psCon.SetPlayerAtk();                   //攻撃力を各コライダーに設定。
                    break;
                }
                break;

            case Effect_List.INVINCIBLE:
                Debug.Log(item.GetName() + "を使用して" + item.GetTime() + "秒間だけ無敵状態になります。");
                edCon.SetTime("inv", item.GetTime());   //効果時間の設定
                psCon.SetInvincible(1);                 //無敵状態にする
                edCon.SetBool("inv");                   //効果時間の測定開始
                break;

            default:
                Debug.Log("NULL");
                break;
        }

        if (!_use) return _use;

        index = Inventry.instance.items.GetItems().IndexOf(item);


        panel.SetActive(false);
        _isDisplay = false;
        Inventry.instance.Remove(slotNum);//該当するアイテムではなく、スロットの番号でアイテムを削除する

        PlayerStatusControl.instance.StatusUpdate();
        Inventry.instance.UpdateUI();


        // Register のリスト内インデックスを取得
        int registerIndex = Register.instance.GetNumList().FindIndex(num => num == slotNum);

        Debug.Log("使用したアイテムのregisterのインデックス : " + registerIndex);

        //登録されていない場合-1になるので、それで登録されているかを判断する
        if(registerIndex != -1)
        {
            Register.instance.RemoveItem(registerIndex);
        }

        return _use;
    }

    //自分のスロットのアイテムを返す
    public Item GetItem()
    {
        return item;
    }

    //スロット番号を渡す
    public int GetSlotNum()
    {
        return slotNum;
    }


    public void DisplayButton()
    {
        if (item == null && !_isDisplay) return;
        if (!_isDisplay && !panel.activeSelf)//他にパネルが表示されておらず、パネルを表示しようとした場合
        {
            _isDisplay = true;
            panel.SetActive(!panel.activeSelf);
        }
        else if(_isDisplay && panel.activeSelf)//パネルを表示している状態でパネルを消す場合
        {
            _isDisplay = false;
            panel.SetActive(!panel.activeSelf);
        }
        //他にパネルが表示されており、もう一つパネルを表示しようとしたとき
    }
}
