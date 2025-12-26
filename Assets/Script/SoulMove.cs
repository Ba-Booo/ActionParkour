using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulMove : MonoBehaviour
{

    private Rigidbody2D rb;
    private ParticleSystem soulParticle;
    [SerializeField] private float soulSpeed;
    [SerializeField] private Transform target;
    [SerializeField] private PlayerControl playerControl;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        soulParticle = GetComponent<ParticleSystem>();
    }

    void LateUpdate()
    {

        transform.rotation = Quaternion.Euler( 0, 0, playerControl.angle - 90 );

        if( ( Input.GetKeyDown( KeyCode.E ) || Input.GetMouseButtonDown(1) ) && playerControl.usingSoul )
        {
            soulParticle.Play();
            rb.AddRelativeForce( Vector3.up * soulSpeed, ForceMode2D.Impulse );
        }
        else if( !playerControl.usingSoul )
        {
            transform.position = Vector2.Lerp( transform.position, target.position, 5f * Time.deltaTime );
        }

    }

    void OnTriggerStay2D( Collider2D collider )
    {

        //점프관련
        if( collider.gameObject.tag == "Player" && !playerControl.usingSoul )
        {
            soulParticle.Stop();
        }
        
    }

}
