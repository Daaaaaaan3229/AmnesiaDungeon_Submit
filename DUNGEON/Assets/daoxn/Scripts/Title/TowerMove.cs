using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerMove : MonoBehaviour
{
    [Header("一番上の位置")]
    [SerializeField]
    private float startPos;

    [Header("一番下の位置")]
    [SerializeField]
    private float endPos;

    [Header("移動速度")]
    [SerializeField]
    private float speed;

    void Update()
    {
        //下方向への移動
        transform.Translate(0, -speed * Time.deltaTime, 0);

        //endPosを越えたら上に戻す
        if(transform.localPosition.y <= endPos)
        {
            //startPosへ移動
            transform.localPosition = new Vector3(transform.localPosition.x, startPos, transform.localPosition.z);

            //いくらか回す
            transform.rotation = Quaternion.Euler(0, Random.Range(0, 90.0f), 0);
        }
    }
}
