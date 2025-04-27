using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackColliderScript : MonoBehaviour
{
    private BossMoveScript bossMoveScript;

    private void Start()
    {
        bossMoveScript = FindObjectOfType<BossMoveScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<IDamagable>(out var damage))
        {
            damage.AddDamage(bossMoveScript.atk);
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("Hit");
        if (other.TryGetComponent<IDamagable>(out var damage))
        {
            damage.AddDamage(bossMoveScript.atk * 1.5f);
        }
    }
}
