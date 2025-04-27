using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatusData", menuName = "ScriptableObjects/CreateEnemyStatusData")]
public class EnemyStatus : ScriptableObject
{
    [field: SerializeField] public string EnemyName { get; private set; }
    [field:SerializeField] public float Hp {  get; private set; }
    [field:SerializeField] public float Atk {  get; private set; }
    [field:SerializeField] public float Def { get; private set; }
}
