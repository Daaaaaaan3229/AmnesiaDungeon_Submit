using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleAttackArea : MonoBehaviour
{
    private GameObject enemyParent;
    private GameObject Player;

    private void Start()
    {
        // �e�I�u�W�F�N�g���擾
        enemyParent = transform.parent.gameObject;

        // �v���C���[�I�u�W�F�N�g���^�O�Ŏ擾
        Player = GameObject.FindWithTag("Player");
    }


    private void OnTriggerEnter(Collider other)
    {
        // �v���C���[���͈͂ɓ�������
        if (other.gameObject == Player)
        {
            enemyParent.GetComponent<TurtleController>().Attack();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // �v���C���[���͈͂���o����
        if (other.gameObject == Player)
        {
            enemyParent.GetComponent<TurtleController>().StopAttack();
        }
    }
}
