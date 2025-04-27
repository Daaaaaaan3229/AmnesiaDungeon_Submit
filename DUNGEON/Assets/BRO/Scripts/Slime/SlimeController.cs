using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeController : EnemyController
{
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
}
