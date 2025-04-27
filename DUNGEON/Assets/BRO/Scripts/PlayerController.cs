using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController: MonoBehaviour
{
    float moveSpeed = 10f;
    float rotateSpeed = 10f;

    void Update()
    {
        //���������Ɛ��������̓��͂��擾���A���ꂼ��̈ړ����x��������B
        float Xvalue = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float Yvalue = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        //���݂�X,Z�x�N�g���ɏ�̏����Ŏ擾�����l��n���B
        Vector3 movedir = new Vector3(Xvalue, 0, Yvalue);

        //���ݒn�ɏ�Ŏ擾�������l�𑫂��Ĉړ�����B
        transform.position += movedir;

        //�i�ޕ����Ɋ��炩�Ɍ����B
        transform.forward = Vector3.Slerp(transform.forward, movedir, Time.deltaTime * rotateSpeed);
    }
}