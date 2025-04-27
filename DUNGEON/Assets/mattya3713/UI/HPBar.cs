using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    [SerializeField]
    private Image collarImage;
    private Image backImage;

    private bool mouseButton = false;

    // Start is called before the first frame update
    void Start()
    {
        // Canvas内でPlayerHPBarBlackを検索
        backImage = GameObject.Find("PlayerHPBarBlack").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

        // Fill Amountによってゲージの色を変える
        if (backImage.fillAmount > 0.90f)
        {
            //赤色
            collarImage.color = Color.red;
        }
        else if (backImage.fillAmount > 0.75f)
        {
            //オレンジ
            collarImage.color = new Color(1f, 0.67f, 0f);
        }
        else
        {
            //緑
            collarImage.color = Color.green;
        }

        // マウスを使ってゲージを増減させる
        if (Input.GetKey(KeyCode.Space))
        {
            mouseButton = true;
        }
        else
        {
            mouseButton = false;
        }

        if (mouseButton)
        {
            backImage.fillAmount += Time.deltaTime / 10;
        }
        else if (backImage.fillAmount > 0f)
        {
            backImage.fillAmount -= Time.deltaTime;
        }
        Debug.Log(backImage.fillAmount);
    }
}