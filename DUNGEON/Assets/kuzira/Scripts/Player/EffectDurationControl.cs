using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDurationControl : MonoBehaviour
{
    private const int normal = 1;    //基本のスタミナ減少率

    //アイテムによる効果時間のあるものを管理
    private bool _st;         //スタミナ値が変化しているかどうか
    private bool _atk;          //攻撃力が変化しているかどうか
    private bool _invincible;   //無敵になっているかどうか

    private float duration_st;          //スタミナ効果時間
    private float duration_atk;         //攻撃力効果時間
    private float duration_invincible;  //無敵時間
    
    //時間計測用
    private float st_time;         
    private float atk_time;
    private float invincible_time;

    private bool _isEffect;     //効果時間中かどうか　効果時間内 : True 効果時間外 : False

    private PlayerStatusControl psCon;  //PlayerStatusControlのインスタンスの作成

    private PlayerStatus ps;            //PlayerStatusの取得用(RW)
    private PlayerStatus ps_max;        //PlayerStatusの取得用(R)

    static public EffectDurationControl instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        psCon = PlayerStatusControl.instance;
        ps = psCon.GetPlayerStatusSO("current");
        ps_max = psCon.GetPlayerStatusSO("max");
    }

    private void Update()
    {
        if(_st || _atk || _invincible)_isEffect = true;
        DurationTimer();
    }

    /// <summary>
    /// _isEffectがTrueの間(効果時間中)timeに加算していく。
    /// </summary>
    private void DurationTimer()
    {
        if (_isEffect)
        {
            if(_st)st_time += Time.deltaTime;
            if(_atk)atk_time += Time.deltaTime;
            if(_invincible)invincible_time += Time.deltaTime;
        }

        if (st_time > duration_st && _st == true)
        {
            //最後に効果時間が切れたものが_isEffectをFalseにする。
            if(_atk == false && _invincible == false)_isEffect = false;
            _st = false;
            psCon.ResetAmpStamina();
            st_time = 0;
        }

        if (atk_time > duration_atk && _atk == true)
        {
            if(_st == false && _invincible == false) _isEffect = false;
            _atk = false;
            atk_time = 0;
            psCon.SetPlayerAtk();
        }

        if (invincible_time > duration_invincible && _invincible == true)
        {
            if(_atk == false && _st == false) _isEffect = false;
            _invincible = false;
            invincible_time = 0;
            psCon.SetInvincible(0);
        }
    }

    /// <summary>
    /// 変化させるステータスに対応したBoolean変数にTrueを設定
    /// </summary>
    /// <param name="name">st atk inv</param>
    public void SetBool(string name)
    {
        switch (name)
        {
            case "st":
                _st = true;
                st_time = 0;
                break;
            case "atk":
                _atk = true;
                atk_time = 0;
                break;
            case "inv":
                _invincible = true;
                invincible_time = 0;
                break;
        }
    }

    /// <summary>
    /// 現在の効果状況を確認する。
    /// </summary>
    /// <param name="name">スタミナ:st  攻撃力:atk  無敵:inv</param>
    /// <returns></returns>
    public bool GetBool(string name)
    {
        switch (name)
        {
            case "st":
                return _st;
            case "atk":
                return _atk;
            case "inv":
                return _invincible;
            default:
                return false;
        }

    }

    public void SetTime(string name, float value)
    {
        switch (name)
        {
            case "st":
                duration_st = value;
                return;
            case "atk":
                duration_atk = value;
                return;
            case "inv":
                duration_invincible = value;
                return;
            default:
                return;
        }
    }
}
