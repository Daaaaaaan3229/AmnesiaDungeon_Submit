using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChildScript : EnemyController
{
    private MushroomController parent; // �e�𔭎˂���MushroomController�̎Q�Ƃ�ێ�

    // MushroomController�̎Q�Ƃ�ݒ肷�郁�\�b�h
    public void SetParent(MushroomController mushroomController)
    {
        parent = mushroomController;
    }

    void Start()
    {
        Initialize();
    }

    void Update()
    {
        //�ړI�n�ɓ��B�������𔻒肷��
        if (ReachDestination())
        {
            //�B��������V�����ړI�n��ݒ肷��
            SetDestination();
        }
        else if (isRun)
        {
            //�v���C���[�Ɍ����킹��
            m_navMeshAgent.SetDestination(playerObj.transform.position);
        }

        SetAnimation();
    }

    protected override void Die()
    {
        //�ړ����~�߂�
        m_navMeshAgent.isStopped = true;
        rb.constraints = RigidbodyConstraints.FreezeRotationY;

        //�폜�ΏۂɎ�����ݒ�
        Destroy(this.gameObject, 0.4f);
    }

}

