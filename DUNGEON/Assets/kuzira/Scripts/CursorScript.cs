using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorScript : MonoBehaviour
{
    private bool onCursorLock;//カーソルを表示するか否かを切り替える true:表示する false:非表示
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
    /// カーソルをオンにする
    /// </summary>
    public void OnCursor()
    {
        onCursorLock = true;
    }

    /// <summary>
    /// カーソルをオフにする
    /// </summary>
    public void OffCursor()
    {
        onCursorLock = false;
    }
}
