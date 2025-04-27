using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class EnemyTest : MonoBehaviour
{
    [SerializeField] [Header("プレイヤーオブジェクト")] protected GameObject playerObject;
    void Start()
    {
            this.gameObject.GetComponent<EnemyController>().SetParameter(playerObject, 1);
    }
}
