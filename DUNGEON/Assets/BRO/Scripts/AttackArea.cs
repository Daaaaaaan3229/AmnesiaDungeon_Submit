using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    private EnemyController enemyController;

    private void Start()
    {
        //�e�I�u�W�F�N�g��EnemyController�R���|�l���g���擾
        enemyController = transform.parent.gameObject.GetComponent<EnemyController>();

        //EnemyController���擾���Ă���Collider���I���ɂ���
        GetComponent<Collider>().enabled = true;

    }

    private void OnTriggerEnter(Collider other)
    {
        // �v���C���[���͈͂ɓ�������
        //�^�O��Player�ł��邩�𔻕�
        if (other.CompareTag("Player"))
        {
            enemyController.Attack();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // �v���C���[���͈͂���o����
        if (other.CompareTag("Player"))
        {
            enemyController.StopAttack();
        }
    }
}
