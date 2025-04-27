using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShotScript : MonoBehaviour
{
    [SerializeField] [Header("弾速")] float speed;
    [SerializeField] [Header("攻撃を当てたときのエフェクト")] public GameObject AttackEffect;
    private CactusController shooter; // 弾を発射したCactusControllerの参照を保持

    // CactusControllerの参照を設定するメソッド
    public void SetShooter(CactusController cactusController)
    {
        shooter = cactusController;
    }

    void FixedUpdate()
    {
        this.transform.position += this.transform.forward * speed * Time.deltaTime; //ショットが前進 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") // プレイヤーに接触したとき 
        {
            shooter.AddAtk(); // CactusControllerのAddAtkメソッドを呼び出す
            this.GetComponent<MeshRenderer>().enabled = false;
            speed = 0;
            Destroy(this, 0.5f);
        } 
        else if (other.tag == "Wall")
        {

            Destroy(this.gameObject, 0.1f);
        }
    }
}

