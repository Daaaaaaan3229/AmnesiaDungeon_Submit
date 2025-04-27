using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutManage : MonoBehaviour
{
    [SerializeField]
    [Header("DungeonManager")]
    private DungeonManager dungeonManager;

    /// <summary>
    /// DungeonManagerからプレイヤー死亡シーンに遷移
    /// </summary>
    public void ChangeDieScene()
    {
        dungeonManager.ChangeDieScene();
    }
}
