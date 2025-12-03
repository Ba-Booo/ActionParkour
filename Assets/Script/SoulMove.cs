using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulMove : MonoBehaviour
{

    private Rigidbody2D rb;
    [SerializeField] private float soulSpeed;

    void Start()
    {

        rb = GetComponent<Rigidbody2D>();

        rb.AddRelativeForce( Vector3.up * soulSpeed, ForceMode2D.Impulse );

        

    }

    void Update()
    {
        
    }

}
