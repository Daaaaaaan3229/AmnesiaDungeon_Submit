using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusControl : MonoBehaviour, IDamagable
{
    [Header("プレイヤーのステータスの最大値設定用のScriptableObject(ReadOnly)")]
    [SerializeField]
    private PlayerStatus pStatus_R;

    [Header("プレイヤーのステータスの変動する値の記録用ScriptableObject(Read and Write)")]
    [SerializeField]
    private PlayerStatus pStatus_RW;

    [SerializeField]
    [Header("プレイヤーの体力表示用ImageGameObject")]
    private GameObject hp_Obj;

    private Image hp_Image;     //hp_ObjのImageコンポーネント取得用

    [SerializeField]
    [Header("プレイヤーのスタミナ表示用のImageGameObject")]
    private GameObject st_Obj;

    private PlayerCharacter[] pcs;  //子オブジェクトのPlayerCharacterの取得

    private EffectDurationControl edCon;    //EffectDurationControlのインスタンス取得用

    private float amp_AtkAll;   //プレイヤーのすべての攻撃力に対する倍率。Slotから設定される。

    private float next_Atk;     //プレイヤーの次の攻撃の時に与えるダメージの値。Slotから設定される。

    private bool _nextUp;       //True:次の攻撃時にnwxt_Atkだけダメージ False:他の攻撃力が設定される。

    [SerializeField]
    [Header("攻撃1段階目の攻撃に対する倍率")]
    private float amp_Atk1;

    [SerializeField]
    [Header("攻撃2段階目の攻撃に対する倍率")]
    private float amp_Atk2; 

    [SerializeField]
    [Header("攻撃3段階目の攻撃に対する倍率")]
    private float amp_Atk3; 

    [SerializeField]
    [Header("攻撃4段階目の攻撃に対する倍率")]
    private float amp_Atk4;

    private Image st_Image;     //st_Objのコンポーネント取得用

    private PlayerStatusDisplay psDisplay;  //PlayerStatusDisplayのインスタンス参照用
    private GoldExpDisplay geDis;           //GoldExpDisplayのインスタンス参照用

    static public  PlayerStatusControl instance;     //他のスクリプトから参照するためのインスタンス

    private float hp;   //プレイヤーのHP

    private AudioSource audioSource;
    [SerializeField]
    [Header("ダメージを受けたときの音")]
    private AudioClip damageClip;
    [SerializeField]
    [Header("ダウンしたときの音")]
    private AudioClip downClip;

    private bool _invincible;   //無敵状態かどうか。TRUE:無敵　FALSE:通常

    private float max_st = 0;   //スタミナの上限を一時的に上げた場合のもとのスタミナ

    [SerializeField]
    [Header("一度に上げられるスタミナの量")]
    private float gain_st;
    [SerializeField]
    [Header("一度に上げられるHPの量")]
    private float gain_hp;
    [SerializeField]
    [Header("一度に上げられる攻撃力の量")]
    private float gain_atk;

    [SerializeField]
    [Header("スタミナを上げるたびに失う経験値の量")]
    private float lost_st;
    [SerializeField]
    [Header("HPを上げるたびに失う経験値の量")]
    private float lost_hp;
    [SerializeField]
    [Header("攻撃力を上げるたびに失う経験値の量")]
    private float lost_atk;

    private bool inVillage; //村にいるとき : True それ以外 : False

    private DropItemScript dropItem; //DropItemScriptのインスタンスを参照する

    private bool _die;  //プレイヤーが倒されたときの動作を一度だけ行うためのBool

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        inVillage = false;
    }

    private void Start()
    {
        if(hp_Obj != null)
        {
            hp_Image = hp_Obj.GetComponent<Image>();
            st_Image = st_Obj.GetComponent<Image>();
        }


        psDisplay = GetComponent<PlayerStatusDisplay>();
        audioSource = GetComponent<AudioSource>();

        pcs = GetComponentsInChildren<PlayerCharacter>();
        edCon = EffectDurationControl.instance;
        geDis = GoldExpDisplay.instance;

        dropItem = GetComponent<DropItemScript>();

        SetPlayerAtk();
    }

    private void Update()
    {
        if(hp_Image != null)
        {
            hp_Image.fillAmount = pStatus_RW.Get("hp") / pStatus_R.Get("hp");
            st_Image.fillAmount = pStatus_RW.Get("st") / pStatus_R.Get("st");
        }

    }

    public float GetCost(string name)
    {
        if(name == "st")
        {
            return lost_st;
        }
        else if(name == "hp")
        {
            return lost_hp;
        }
        else if(name == "atk")
        {
            return lost_atk;
        }
        else
        {
            return 0;
        }
    }

    public float GetGain(string name)
    {
        if (name == "st")
        {
            return gain_st;
        }
        else if (name == "hp")
        {
            return gain_hp;
        }
        else if (name == "atk")
        {
            return gain_atk;
        }
        else
        {
            return 0;
        }
    }

    /// <summary>
    /// 外部から村にいるときの処理を行うため
    /// </summary>
    /// <param name="set">村にいるとき : True それ以外 : False</param>
    public void SetInVillage(bool set)
    {
        inVillage = set;
    }

    public bool GetInVillage()
    {
        return inVillage;
    }

    /// <summary>
    /// pStatusからプレイヤーの設定ステータスを取得
    /// </summary>
    public void StatusSet()
    {
        pStatus_RW.Set("hp", pStatus_R.Get("hp"));
        pStatus_RW.Set("st", pStatus_R.Get("st"));
        pStatus_RW.Set("atk", pStatus_R.Get("atk"));
        pStatus_RW.Set("mag", pStatus_R.Get("mag"));
        pStatus_RW.Set("def", pStatus_R.Get("def"));
        pStatus_RW.Set("spd", pStatus_R.Get("spd"));
        pStatus_RW.Set("lv", pStatus_R.Get("lv"));
    }

    /// <summary>
    /// それぞれの攻撃ごとに、攻撃力を設定する。
    /// </summary>
    public void SetPlayerAtk()
    {
        int i = 0;

        if (_nextUp)
        {
            for (i = 0; i < pcs.Count(); i++)
            {
                pcs[i].SetPlayerAtk(next_Atk);
            }
        }

        if(edCon.GetBool("atk") == true)
        {
            for(i = 0; i < pcs.Count(); i++)
            {
                if (i == 0) pcs[i].SetPlayerAtk(pStatus_RW.Get("atk") * amp_Atk1 * amp_AtkAll);
                else if (i == 1) pcs[i].SetPlayerAtk(pStatus_RW.Get("atk") * amp_Atk2 * amp_AtkAll);
                else if (i == 2) pcs[i].SetPlayerAtk(pStatus_RW.Get("atk") * amp_Atk3 * amp_AtkAll);
                else if (i == 3) pcs[i].SetPlayerAtk(pStatus_RW.Get("atk") * amp_Atk4 * amp_AtkAll);
            }
            return;
        }

        for(i = 0 ; i<pcs.Count() ; i++)
        {
            if(i == 0) pcs[i].SetPlayerAtk(pStatus_RW.Get("atk") * amp_Atk1);
            else if(i == 1) pcs[i].SetPlayerAtk(pStatus_RW.Get("atk") * amp_Atk2);
            else if(i == 2) pcs[i].SetPlayerAtk(pStatus_RW.Get("atk") * amp_Atk3);
            else if(i == 3) pcs[i].SetPlayerAtk(pStatus_RW.Get("atk") * amp_Atk4);
        }
    }

    public void SetAmpAtkAll(float value)
    {
        amp_AtkAll = value;
    }
    public void SetNextUp(bool value)
    {
        _nextUp = value;
    }
    public bool GetNextUp()
    {
        return _nextUp;
    }
    public void SetNextAtk(float value)
    {
        next_Atk = value;
    }

    public void StatusCalculate(string name, float value)
    {
        switch (name)
        {
            case "st":
                pStatus_RW.Set("st", pStatus_RW.Get("st") + value);
                break;
            case "hp":
                pStatus_RW.Set("hp", pStatus_RW.Get("hp") + value);
                break;
            case "atk":
                pStatus_RW.Set("atk", pStatus_RW.Get("atk") + value);
                break;
            case "mag":
                pStatus_RW.Set("mag", pStatus_RW.Get("mag") + value);
                break;
            case "def":
                pStatus_RW.Set("def", pStatus_RW.Get("def") + value);
                break;
            case "spd":
                pStatus_RW.Set("spd", pStatus_RW.Get("spd") + value);
                break;
            case "lv":
                pStatus_RW.Set("lv", pStatus_RW.Get("lv") + value);
                break;
            case "exp":
                pStatus_RW.Set("exp", pStatus_RW.Get("exp") + value);
                break;
            case "gold":
                pStatus_RW.Set("gold", pStatus_RW.Get("gold") + value);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// ステータスの上限に関する計算
    /// </summary>
    /// <param name="name">変更する項目</param>
    /// <param name="value">変更する値</param>
    public void StatusCalculateMax(string name, float value)
    {
        switch (name)
        {
            case "st":
                pStatus_R.Set("st", pStatus_R.Get("st") + value);
                break;
            case "hp":
                pStatus_R.Set("hp", pStatus_R.Get("hp") + value);
                break;
            case "atk":
                pStatus_R.Set("atk", pStatus_R.Get("atk") + value);
                break;
            case "mag":
                pStatus_R.Set("mag", pStatus_R.Get("mag") + value);
                break;
            case "def":
                pStatus_R.Set("def", pStatus_R.Get("def") + value);
                break;
            case "spd":
                pStatus_R.Set("spd", pStatus_R.Get("spd") + value);
                break;
            case "lv":
                pStatus_R.Set("lv", pStatus_R.Get("lv") + value);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// ステータスの割り振り
    /// </summary>
    /// <param name="name">ステータスの項目</param>
    public void StatusUp(string name)
    {
        switch (name)
        {
            case "st":
                if(pStatus_RW.Get("exp") >= Mathf.Abs(lost_st))
                {
                    StatusCalculateMax("st", gain_st);
                    StatusCalculate("exp", lost_st);
                    geDis.Display();
                }
                break;
            case "atk":
                if(pStatus_RW.Get("exp") >= Mathf.Abs(lost_atk))
                {
                    StatusCalculateMax("atk", gain_atk);
                    StatusCalculate("exp", lost_atk);
                    geDis.Display();
                }
                break;
            case "hp":
                if(pStatus_RW.Get("exp") >= Mathf.Abs(lost_hp))
                {
                    StatusCalculateMax("hp", gain_hp);
                    StatusCalculate("exp", lost_hp);
                    geDis.Display();
                }
                break;
            default:
                break;
        }

        StatusUpdate();

    }

    /// <summary>
    /// スタミナが尽きていないかの処理
    /// </summary>
    /// <returns></returns>
    public bool StaminaCon()
    {
        if (pStatus_RW.Get("st") == 0) return false;
        else return true;
    }

    public void StaminaCalculate(int sign)
    {
        //signが - なら減少。 + なら増加させる。
        StatusCalculate("st", Time.deltaTime * sign);

        //スタミナが最大値よりも大きくなった場合は、スタミナの値を最大値で上書き。
        //スタミナが最低値よりも下になった場合、スタミナの値を0に。
        if (pStatus_RW.Get("st") > pStatus_R.Get("st")) pStatus_RW.Set("st", pStatus_R.Get("st"));
        else if (pStatus_RW.Get("st") < 0) pStatus_RW.Set("st", 0);

        //ステータス表示の更新
        StatusUpdate();
    }

    public void SetAmpStamina(float amp)
    {
        if (max_st == 0)
        {
            max_st = pStatus_R.Get("st");
            pStatus_R.Set("st", pStatus_R.Get("st") * amp);
        }
        else
        {
            pStatus_R.Set("st", max_st * amp);
        }
    }

    public void ResetAmpStamina()
    {
        pStatus_R.Set("st", max_st);
        max_st = 0;
    }

    public void StatusUpdate()
    {
        psDisplay.StatusUpdate();
    }

    public void AddDamage(float damage)
    {
        if (_invincible == false)
        {
            pStatus_RW.Set("hp", pStatus_RW.Get("hp") - damage);
            audioSource.PlayOneShot(damageClip);
            Debug.Log("プレイヤーは" + damage + "のダメージを受けた");
        }
        else
        {
            Debug.Log("回避!");
        }

        //HPが0いかになった時の処理
        if(pStatus_RW.Get("hp") <= 0 && _die == false)
        {
            _die = true;
            audioSource.PlayOneShot(downClip);

            dropItem.DropPlayer();  //プレイヤーが倒された時の処理

            //DungeonManagerを探し、処理をする
            GameObject.Find("DungeonManager").GetComponent<DungeonManager>().PlayerDie();
        }

    }

    public float GetStatus(string name)
    {
        if (name == "st") return pStatus_RW.Get("st");
        else if (name == "full_St") return pStatus_R.Get("st");
        else return 0;
    }

    public PlayerStatus GetPlayerStatusSO(string name)
    {
        switch (name)
        {
            case "max":
                return pStatus_R;
            case "current":
                return pStatus_RW;
            default:
                return null;
        }
    }

    /// <summary>
    /// DungeonManagerから減った分のHPを受け取る、プロト版でとりあえずの実装。
    /// </summary>
    public void GetHPManager(float getHP)
    {
        hp = getHP;
    }

    /// <summary>
    /// DungeonManagerに減った分のHPを渡す、プロト版でとりあえずの実装。
    /// </summary>
    public float SetHPManager()
    {
        return hp;
    }

    /// <summary>
    /// 無敵状態かどうかの設定
    /// </summary>
    /// <param name="value">0:FALSE  それ以外:TRUE</param>
    public void SetInvincible(int value)
    {
        if(value == 0 && edCon.GetBool("inv") == false)_invincible = false;
        else _invincible = true;
    }

    public bool GetInvincible()
    {
        return _invincible;
    }
}
