using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//�v���g�łł̃V�[���J�ڃX�N���v�g

public class SceneChangeProto : MonoBehaviour
{
    [SerializeField]
    [Header("�J�ڂ������V�[����")]
    private string sceneName;

    /// <summary>
    /// �{�^���������ꂽ��A�V�[���J�ڂ���
    /// </summary>
    public void OnBuuton()
    {
        SceneManager.LoadScene(sceneName);
    }
}
