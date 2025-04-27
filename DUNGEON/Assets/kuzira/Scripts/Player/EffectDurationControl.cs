using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDurationControl : MonoBehaviour
{
    private const int normal = 1;    //��{�̃X�^�~�i������

    //�A�C�e���ɂ����ʎ��Ԃ̂�����̂��Ǘ�
    private bool _st;         //�X�^�~�i�l���ω����Ă��邩�ǂ���
    private bool _atk;          //�U���͂��ω����Ă��邩�ǂ���
    private bool _invincible;   //���G�ɂȂ��Ă��邩�ǂ���

    private float duration_st;          //�X�^�~�i���ʎ���
    private float duration_atk;         //�U���͌��ʎ���
    private float duration_invincible;  //���G����
    
    //���Ԍv���p
    private float st_time;         
    private float atk_time;
    private float invincible_time;

    private bool _isEffect;     //���ʎ��Ԓ����ǂ����@���ʎ��ԓ� : True ���ʎ��ԊO : False

    private PlayerStatusControl psCon;  //PlayerStatusControl�̃C���X�^���X�̍쐬

    private PlayerStatus ps;            //PlayerStatus�̎擾�p(RW)
    private PlayerStatus ps_max;        //PlayerStatus�̎擾�p(R)

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
    /// _isEffect��True�̊�(���ʎ��Ԓ�)time�ɉ��Z���Ă����B
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
            //�Ō�Ɍ��ʎ��Ԃ��؂ꂽ���̂�_isEffect��False�ɂ���B
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
    /// �ω�������X�e�[�^�X�ɑΉ�����Boolean�ϐ���True��ݒ�
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
    /// ���݂̌��ʏ󋵂��m�F����B
    /// </summary>
    /// <param name="name">�X�^�~�i:st  �U����:atk  ���G:inv</param>
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
