using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class sutaminaBar : MonoBehaviour
{
    public float speed = 5.0f;
    public Slider slider;

    float sutamina;

    void Start()
    {
        // Canvas����StaminaBar������
        slider = GameObject.Find("StaminaBar").GetComponent<Slider>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            sutamina = sutamina + (Time.deltaTime / 2);
        }

        //�X�^�~�i���O�ȏ�̎�
        else if (sutamina > 0f)
        {
            //�X�^�~�i�����X�Ɍ���
            sutamina = sutamina - (Time.deltaTime / 2); ;
        }


        if (sutamina > 1f)
        {
            sutamina = 1f;
        }

        Debug.Log(slider.value);


        slider.value = sutamina;
    }
}