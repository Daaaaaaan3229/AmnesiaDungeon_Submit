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
        // Canvas内でStaminaBarを検索
        slider = GameObject.Find("StaminaBar").GetComponent<Slider>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            sutamina = sutamina + (Time.deltaTime / 2);
        }

        //スタミナが０以上の時
        else if (sutamina > 0f)
        {
            //スタミナが徐々に減る
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