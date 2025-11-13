using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{

    [SerializeField] private GameObject target;

    //카메라 이동 범위
    public Vector2 xRange;
    public Vector2 yRange;

    //대쉬관련
    [SerializeField] private PlayerControl playerControl;

    void Start()
    {

    }

    void LateUpdate()
    {

        //카메라 움직임
        transform.position = new Vector2( Mathf.Lerp( transform.position.x, target.transform.position.x, 10f * Time.deltaTime ), transform.position.y );

        //카메라 움직임 범위
        float xClamp = Mathf.Clamp(transform.position.x, xRange.x, xRange.y);

        transform.position = new Vector3(xClamp, transform.position.y, -10f);

    }

    void FixedUpdate()
    {

        //카메라 움직임
        if( playerControl.isDashing )
        {
            transform.position = transform.position = new Vector2( Mathf.Lerp( transform.position.x, target.transform.position.x, 10f * Time.deltaTime ), Mathf.Lerp(transform.position.y, target.transform.position.y, 20f * Time.deltaTime) );
        }
        else
        {
            transform.position = transform.position = new Vector2( transform.position.x, Mathf.Lerp(transform.position.y, target.transform.position.y, 20f * Time.deltaTime) );
        }
        

        //카메라 움직임 범위
        float yClamp = Mathf.Clamp(transform.position.y, yRange.x, yRange.y);

        transform.position = new Vector3(transform.position.x, yClamp, -10f);

    }

}
