using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDetection : MonoBehaviour
{
    protected EnemyController enemyScript; // �e�I�u�W�F�N�g�̃X�N���v�g���i�[����ϐ�

    private void Start()
    {
        // �e�I�u�W�F�N�g����X�N���v�g���擾
        enemyScript = transform.parent.gameObject.GetComponent<EnemyController>();

        if (enemyScript == null)
        {
            Debug.LogError("�e�I�u�W�F�N�g�ɃX�N���v�g��������܂���ł����B");
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        CallAddAtk(other);
    }

    protected void CallAddAtk(Collider other)
    {
        // �v���C���[���͈͂ɓ�������
        if (other.gameObject.CompareTag("Player") && enemyScript != null)
        {
            // �e�I�u�W�F�N�g�̃X�N���v�g����AddAtk()���\�b�h���Ăяo��
            enemyScript.AddAtk();
        }
    }
}
