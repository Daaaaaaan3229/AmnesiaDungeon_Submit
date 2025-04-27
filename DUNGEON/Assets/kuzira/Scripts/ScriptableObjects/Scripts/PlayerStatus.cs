using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="PlayerStatusData", menuName = "ScriptableObjects/CreatePlayerStatusData")]
public class PlayerStatus : ScriptableObject
{
    [SerializeField]
    private float hp;
    [SerializeField]
    private float st;
    [SerializeField]
    private float atk;
    [SerializeField]
    private float mag;
    [SerializeField]
    private float def;
    [SerializeField]
    private float spd;
    [SerializeField]
    private float lv;
    [SerializeField]
    private float exp;
    [SerializeField]
    private float gold;
    /// <summary>
    /// ScriptableObject�̒l�̎擾
    /// </summary>
    /// <param name="name">hp / st / atk / mag / def / spd / lv / exp</param>
    /// <returns>�w�肳�ꂽ�X�e�[�^�X�̒l</returns>
    public float Get(string name)
    {
        switch (name)
        {
            case "hp":
                return hp;
            case "st":
                return st;
            case "atk":
                return atk;
            case "mag":
                return mag;
            case "def":
                return def;
            case "spd":
                return spd;
            case "lv":
                return lv;
            case "exp":
                return exp;
            case "gold":
                return gold;
            default:
                return 0;
        }
    }

    /// <summary>
    /// ScriptableObject�̒l�̐ݒ�
    /// </summary>
    /// <param name="name">hp / st / atk / mag / def / spd / lv / exp</param>
    /// <param name="value">�ݒ肵�����l</param>
    public void Set(string name, float value)
    {
        switch (name)
        {
            case "hp":
                hp = value;
                break;
            case "st":
                st = value;
                break;
            case "atk":
                atk = value;
                break;
            case "mag":
                mag = value;
                break;
            case "def":
                def = value;
                break;
            case "spd":
                spd = value;
                break;
            case "lv":
                lv = value;
                break;
            case "exp":
                exp = value;
                break;
            case "gold":
                gold = value;
                break;
            default:
                break;
        }
    }
}
