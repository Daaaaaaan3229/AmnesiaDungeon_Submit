using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorScript : MonoBehaviour
{
    private bool onCursorLock;//�J�[�\����\�����邩�ۂ���؂�ւ��� true:�\������ false:��\��
    void Start()
    {
        onCursorLock = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (PlayerMenu.instance.GetDisplayStatus() == true || PlayerMenu.instance.GetTalking() == true || onCursorLock == true)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else if(PlayerMenu.instance.GetDisplayStatus() == false && PlayerMenu.instance.GetTalking() == false && onCursorLock == false)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    /// <summary>
    /// �J�[�\�����I���ɂ���
    /// </summary>
    public void OnCursor()
    {
        onCursorLock = true;
    }

    /// <summary>
    /// �J�[�\�����I�t�ɂ���
    /// </summary>
    public void OffCursor()
    {
        onCursorLock = false;
    }
}
