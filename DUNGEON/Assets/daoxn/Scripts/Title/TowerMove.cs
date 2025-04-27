using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerMove : MonoBehaviour
{
    [Header("ˆê”Ôã‚ÌˆÊ’u")]
    [SerializeField]
    private float startPos;

    [Header("ˆê”Ô‰º‚ÌˆÊ’u")]
    [SerializeField]
    private float endPos;

    [Header("ˆÚ“®‘¬“x")]
    [SerializeField]
    private float speed;

    void Update()
    {
        //‰º•ûŒü‚Ö‚ÌˆÚ“®
        transform.Translate(0, -speed * Time.deltaTime, 0);

        //endPos‚ğ‰z‚¦‚½‚çã‚É–ß‚·
        if(transform.localPosition.y <= endPos)
        {
            //startPos‚ÖˆÚ“®
            transform.localPosition = new Vector3(transform.localPosition.x, startPos, transform.localPosition.z);

            //‚¢‚­‚ç‚©‰ñ‚·
            transform.rotation = Quaternion.Euler(0, Random.Range(0, 90.0f), 0);
        }
    }
}
