using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour 
{
    private GameObject atk_target;     //攻撃した相手

    private float playerAtk;            //プレイヤーの攻撃力

    private PlayerStatusControl psCon;  //PlayerStatusControlのインスタンス参照用

    private AudioSource audioSource;    //AudioSourceコンポーネントの取得
    [SerializeField]
    [Header("攻撃が当たった場合の音")]
    private AudioClip hitClip;
    [SerializeField]
    [Header("攻撃が空振りになった時の音")]
    private AudioClip nonehitClip;
    [SerializeField]
    [Header("攻撃時のエフェクト")]
    private GameObject attackEffect;

    private Vector3 hitPos;
    private Transform pTrans;       //プレイヤーのTransformの取得

    private BoxCollider boxCollider;    //コライダーの参照
    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        audioSource = GetComponent<AudioSource>();
        pTrans = GetComponent<Transform>();
        psCon = PlayerStatusControl.instance;
    }

    private void Update()
    {
        if(atk_target != null)
        {
            //この攻撃にダメージ指定がなされていた場合
            if (psCon.GetNextUp() == true)
            {
                psCon.SetNextUp(false);     //次回攻撃時ダメージを定数にしないためのBool設定。
                psCon.SetPlayerAtk();
            }
            Instantiate(attackEffect, hitPos, Quaternion.identity);
            audioSource.PlayOneShot(hitClip);
            atk_target = null;
        }
        else if(atk_target == null && boxCollider.enabled == true && audioSource.isPlaying == false)
        {
            audioSource.PlayOneShot(nonehitClip);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Boss"))
        {
            hitPos = other.ClosestPointOnBounds(pTrans.position);
            atk_target = other.gameObject;
            var damageble = other.gameObject.GetComponent<IDamagable>();
            if (damageble != null)
            {
                damageble.AddDamage(playerAtk);
                Debug.Log("playerAtk");
            }
        }
    }

    public void SetPlayerAtk(float value)
    {
        playerAtk = value;
    }

    public float GetPlayerAtk()
    {
        return playerAtk;
    }
}
