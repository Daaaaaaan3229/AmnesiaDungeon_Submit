using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    [Header("�G�t�F�N�g�ɃA�^�b�`����X�N���v�g")]
    private ParticleSystem ps;
    private void Start()
    {
        ps = this.gameObject.GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if(ps.isStopped)
        {
            Destroy(this.gameObject);
        }
    }
}
