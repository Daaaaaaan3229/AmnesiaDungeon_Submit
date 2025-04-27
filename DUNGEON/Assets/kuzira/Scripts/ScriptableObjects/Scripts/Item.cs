using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObjects/Create Item")]    

public class Item : ScriptableObject
{
    [SerializeField]
    [Header("�A�C�e���̖��O")]
    new private string name = "Item Name";

    [SerializeField]
    [Header("�A�C�e���̃A�C�R��")]
    private Sprite icon = null;

    [SerializeField]
    [Header("�A�C�e���̌��ʂ̑I��")]
    private Effect_List currentState;

    [SerializeField]
    [Header("�A�C�e���̌��ʎ��Ԃ̐ݒ�B���� or ����")]
    private Effect_Time timeState;

    [SerializeField]
    [Header("�A�C�e���̌��ʗʂ�ݒ�")]
    private float value;

    [SerializeField]
    [Header("�A�C�e���̌��ʎ��Ԃ�ݒ�\n�A�C�e���̌��ʂ��������̕��Ȃ�ݒ�̕K�v�Ȃ��B")]
    private float time;

    [SerializeField]
    [Header("�A�C�e�����w������ۂɕK�v�ȋ��z")]
    private float gold;

    public Effect_List GetCurrentState()
    {
        return currentState;
    }

    public Effect_Time GetTimeState()
    {
        return timeState;
    }

    public Sprite GetIcon()
    {
        return icon;
    }

    public float GetValue()
    {
        return value;
    }

    public float GetTime()
    {
        return time;
    }

    public string GetName()
    {
        return name;
    }

    public float GetGold()
    {
        return gold;
    }
}
public enum Effect_List
{
    HP_Recover,
    ST_Recover,
    MAG_Recover,
    ATK_UP,
    INVINCIBLE
}

public enum Effect_Time
{
    Instant,
    Continuation,
    Next
}
