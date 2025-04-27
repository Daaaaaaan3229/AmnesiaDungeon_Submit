using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationPointManager : MonoBehaviour
{
    public GameObject targetObject;  // インスペクタで設定する接触対象のオブジェクト

    void OnTriggerEnter(Collider collision)
    {
        // 対象のオブジェクトに接触したとき
        if (collision.gameObject == targetObject)
        {
            // ランダムなx座標とz座標を生成し、y座標を1に設定
            float randomX = Random.Range(-10f, 10f);
            float randomZ = Random.Range(-10f, 10f);
            float fixedY = 1f;

            // 新しい位置にワープさせる
            transform.position = new Vector3(randomX, fixedY, randomZ);
        }
    }
}
