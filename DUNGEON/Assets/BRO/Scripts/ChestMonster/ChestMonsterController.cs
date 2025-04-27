using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestMonsterController : EnemyController, IDamagable
{
    void Start()
    {
        Initialize();
        m_navMeshAgent.isStopped = true;//�v���C���[���߂Â��܂ł͐Î~
    }

    void Update()
    {
        //�ړI�n�ɓ��B�������𔻒肷��
        if (ReachDestination())
        {
            //�B��������V�����ړI�n��ݒ肷��
            SetDestination();
        }
        SetAnimation();

        //�v���C���[���߂Â��Ɠ����o��
        if(isRun)
        {
            m_navMeshAgent.isStopped = false;
        }
        //�����o���Ə�Ƀv���C���[��ǂ�������
        if(!m_navMeshAgent.isStopped)
        {
            m_navMeshAgent.SetDestination(playerObj.transform.position);
        }
    }
}
