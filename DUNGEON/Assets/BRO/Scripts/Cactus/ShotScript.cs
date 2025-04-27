using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShotScript : MonoBehaviour
{
    [SerializeField] [Header("�e��")] float speed;
    [SerializeField] [Header("�U���𓖂Ă��Ƃ��̃G�t�F�N�g")] public GameObject AttackEffect;
    private CactusController shooter; // �e�𔭎˂���CactusController�̎Q�Ƃ�ێ�

    // CactusController�̎Q�Ƃ�ݒ肷�郁�\�b�h
    public void SetShooter(CactusController cactusController)
    {
        shooter = cactusController;
    }

    void FixedUpdate()
    {
        this.transform.position += this.transform.forward * speed * Time.deltaTime; //�V���b�g���O�i 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") // �v���C���[�ɐڐG�����Ƃ� 
        {
            shooter.AddAtk(); // CactusController��AddAtk���\�b�h���Ăяo��
            this.GetComponent<MeshRenderer>().enabled = false;
            speed = 0;
            Destroy(this, 0.5f);
        } 
        else if (other.tag == "Wall")
        {

            Destroy(this.gameObject, 0.1f);
        }
    }
}

