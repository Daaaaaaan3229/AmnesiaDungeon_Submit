using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class testMove : MonoBehaviour,IDamagable
{
    private CharacterController characterController;

    private Vector3 input;

    [SerializeField] private float movespeed;
    [SerializeField] private float debugDamage;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        input = new Vector3(UnityEngine.Input.GetAxis("Horizontal"), 0f, UnityEngine.Input.GetAxis("Vertical"));

        characterController.Move(input * Time.deltaTime * movespeed);
    }

    public void AddDamage(float damage)
    {
        Debug.Log("Hit by " + damage);
    }

    private void OnCollisionEnter(Collision collision)
    {
        var damageble = collision.gameObject.GetComponent<IDamagable>();
        if (damageble != null && (Object)damageble != this)
        {
            damageble.AddDamage(debugDamage);
        }
    }

}
