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
        // Canvas����PlayerHPBarBlack������
        backImage = GameObject.Find("PlayerHPBarBlack").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

        // Fill Amount�ɂ���ăQ�[�W�̐F��ς���
        if (backImage.fillAmount > 0.90f)
        {
            //�ԐF
            collarImage.color = Color.red;
        }
        else if (backImage.fillAmount > 0.75f)
        {
            //�I�����W
            collarImage.color = new Color(1f, 0.67f, 0f);
        }
        else
        {
            //��
            collarImage.color = Color.green;
        }

        // �}�E�X���g���ăQ�[�W�𑝌�������
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