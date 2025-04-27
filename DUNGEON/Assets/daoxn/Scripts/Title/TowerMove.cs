using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerMove : MonoBehaviour
{
    [Header("��ԏ�̈ʒu")]
    [SerializeField]
    private float startPos;

    [Header("��ԉ��̈ʒu")]
    [SerializeField]
    private float endPos;

    [Header("�ړ����x")]
    [SerializeField]
    private float speed;

    void Update()
    {
        //�������ւ̈ړ�
        transform.Translate(0, -speed * Time.deltaTime, 0);

        //endPos���z�������ɖ߂�
        if(transform.localPosition.y <= endPos)
        {
            //startPos�ֈړ�
            transform.localPosition = new Vector3(transform.localPosition.x, startPos, transform.localPosition.z);

            //�����炩��
            transform.rotation = Quaternion.Euler(0, Random.Range(0, 90.0f), 0);
        }
    }
}
