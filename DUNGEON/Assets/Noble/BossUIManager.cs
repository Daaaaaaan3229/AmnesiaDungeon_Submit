using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossUIManager : MonoBehaviour
{
    [SerializeField] private Slider hpSlider;
    [SerializeField] private BossMoveScript dragonScript;
    [SerializeField] private GameObject stair;

    [SerializeField]
    [Header("DungeonManagerオブジェクト")]
    private DungeonManager dungeonManager;

    // Start is called before the first frame update
    void Start()
    {
        stair.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        hpSlider.value = dragonScript.hp;

        if(dragonScript.hp <= 0)
        {
            if (stair != null) 
            {
                stair.SetActive(true);

                //BGMのボリュームを少しずつ減らして消す
                dungeonManager.VolumeFadeOut();
            }
            
        }
    }
}
