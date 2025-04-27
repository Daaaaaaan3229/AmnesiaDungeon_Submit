using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationPointManager : MonoBehaviour
{
    public GameObject targetObject;  // �C���X�y�N�^�Őݒ肷��ڐG�Ώۂ̃I�u�W�F�N�g

    void OnTriggerEnter(Collider collision)
    {
        // �Ώۂ̃I�u�W�F�N�g�ɐڐG�����Ƃ�
        if (collision.gameObject == targetObject)
        {
            // �����_����x���W��z���W�𐶐����Ay���W��1�ɐݒ�
            float randomX = Random.Range(-10f, 10f);
            float randomZ = Random.Range(-10f, 10f);
            float fixedY = 1f;

            // �V�����ʒu�Ƀ��[�v������
            transform.position = new Vector3(randomX, fixedY, randomZ);
        }
    }
}
