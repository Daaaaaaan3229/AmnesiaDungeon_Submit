using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCollider : MonoBehaviour,IDamagable
{
    private BossMoveScript dragonScript;

    private void Awake()
    {
        dragonScript = transform.root.GetComponent<BossMoveScript>();
        if(dragonScript == null)
        {
            dragonScript = transform.root.GetChild(0).GetComponent<BossMoveScript>();
        }
    }
    public void AddDamage(float damage)
    {
        if (dragonScript != null)
        {
            dragonScript.Damage(damage);
            Debug.Log("current hp:" + dragonScript.hp);
        }
    }
}
