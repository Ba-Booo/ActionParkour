using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulMove : MonoBehaviour
{

    // private bool collected;
    private Rigidbody2D rb;
    [SerializeField] private float soulSpeed;
    // [SerializeField] private GameObject target;

    void Start()
    {

        rb = GetComponent<Rigidbody2D>();

        rb.AddRelativeForce( Vector3.up * soulSpeed, ForceMode2D.Impulse );

    }

    void Update()
    {
        if( Input.GetKeyDown( KeyCode.E ) )     //보류
        {
            Destroy( this.gameObject );
        }
    }

}
