using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    [SerializeField]
    private Image qBackImage;
    private Text qBackNomber;
    public float qMaxTime;
    private float qNowTime;
    private bool qButton = false;

    [SerializeField]
    private Image eBackImage;
    private Text eBackNomber;
    public float eMaxTime;
    private float eNowTime;
    private bool eButton = false;

    void Start()
    {
        // Canvas内で検索
        qBackImage = GameObject.Find("QFade").GetComponent<Image>();
        qBackImage.fillAmount = 0f;
        qBackNomber = GameObject.Find("QTime").GetComponent<Text>();
        qBackNomber.enabled = false;
        qNowTime = qMaxTime;

        eBackImage = GameObject.Find("EFade").GetComponent<Image>();
        eBackImage.fillAmount = 0f;
        eBackNomber = GameObject.Find("ETime").GetComponent<Text>();
        eBackNomber.enabled = false;
        eNowTime = eMaxTime;
    }

    void Update()
    {
        // マウスを使ってゲージを増減させる
        if (Input.GetKey(KeyCode.Q))
        {
            qButton = true;
            qBackNomber.enabled = true;
        }
        if (Input.GetKey(KeyCode.E))
        {
            eButton = true;
            eBackNomber.enabled = true;
        }

        if (qButton)
        {
            qNowTime -= Time.deltaTime * 60f;
            qBackImage.fillAmount = qNowTime / qMaxTime;

            float timeRemaining = qMaxTime - (qMaxTime - qNowTime);
            float minutes = timeRemaining / 60f;
            float tenths = ((timeRemaining - Mathf.Floor(minutes) * 60f) / 6f) + 0.05f; // 四捨五入のために0.05を加算

            qBackNomber.text = ((int)minutes).ToString() + "." + ((int)tenths).ToString();

            if (qNowTime <= 0f)
            {
                qButton = false;
                qBackNomber.enabled = false;
                qNowTime = qMaxTime;
            }
        }

        if (eButton)
        {
            eNowTime -= Time.deltaTime * 60f;
            eBackImage.fillAmount = eNowTime / eMaxTime;

            float timeRemaining = eMaxTime - (eMaxTime - eNowTime);
            float minutes = timeRemaining / 60f;
            float tenths = ((timeRemaining - Mathf.Floor(minutes) * 60f) / 6f) + 0.05f; // 四捨五入のために0.05を加算

            eBackNomber.text = ((int)minutes).ToString() + "." + ((int)tenths).ToString();

            if (eNowTime <= 0f)
            {
                eButton = false;
                eBackNomber.enabled = false;
                eNowTime = eMaxTime;
            }
        }

        Debug.Log(qButton);
    }
}
