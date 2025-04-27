using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController: MonoBehaviour
{
    float moveSpeed = 10f;
    float rotateSpeed = 10f;

    void Update()
    {
        //垂直方向と水平方向の入力を取得し、それぞれの移動速度をかける。
        float Xvalue = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float Yvalue = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        //現在のX,Zベクトルに上の処理で取得した値を渡す。
        Vector3 movedir = new Vector3(Xvalue, 0, Yvalue);

        //現在地に上で取得をした値を足して移動する。
        transform.position += movedir;

        //進む方向に滑らかに向く。
        transform.forward = Vector3.Slerp(transform.forward, movedir, Time.deltaTime * rotateSpeed);
    }
}