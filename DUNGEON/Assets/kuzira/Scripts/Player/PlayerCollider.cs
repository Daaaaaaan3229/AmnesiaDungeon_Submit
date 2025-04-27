using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    [SerializeField]
    private Collider attack1;

    [SerializeField] 
    private Collider attack2;

    [SerializeField] 
    private Collider attack3;

    [SerializeField] 
    private Collider attack4;

    private Animator anim;

    private void Start()
    {
        attack1.enabled = false;
        attack2.enabled = false;
        attack3.enabled = false;
        attack4.enabled = false;

        anim = GetComponent<Animator>();
    }

    public void OnCollider(float attack_count)
    {
        if(attack_count == 0)
        {
            attack1.enabled = true;
        }
        else if(attack_count == 1)
        {
            attack2.enabled = true;
        }
        else if(attack_count == 2)
        {
            attack3.enabled = true;
        }
        else if(attack_count == 3)
        {
            attack4.enabled = true;
        }
    }

    public void OffCollider(float attack_count)
    {
        if (attack_count == 0)
        {
            attack1.enabled = false;
        }
        else if (attack_count == 1)
        {
            attack2.enabled = false;
        }
        else if (attack_count == 2)
        {
            attack3.enabled = false;
        }
        else if (attack_count == 3)
        {
            attack4.enabled = false;
        }
    }
}
