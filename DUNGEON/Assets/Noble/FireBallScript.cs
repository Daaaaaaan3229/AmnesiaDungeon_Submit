using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FireBallScript : MonoBehaviour
{
    [SerializeField] private float moveValue = 5f;
    // Start is called before the first frame update
    void Start()
    {
        transform.DOMove(transform.forward * moveValue, 3f).SetRelative(true).SetEase(Ease.OutCubic);
        Destroy(gameObject, 1.5f);
    }
}
