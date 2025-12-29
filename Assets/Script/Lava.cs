using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private float distance;
    [SerializeField] private Transform target;

    void Update()
    {
        transform.position = new Vector3( transform.position.x, transform.position.y + speed * Time.deltaTime, transform.position.z );

        if( transform.position.y < target.position.y - distance )
        {
            transform.position = new Vector3( transform.position.x, Mathf.Lerp(transform.position.y, target.position.y - distance, 1.5f * Time.deltaTime ), transform.position.z );
        }

    }
}
