using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    [SerializeField] private float playerSpeed;

    //점프
    private int nowJumpCount;
    [SerializeField] private int maxJumpCount;
    [SerializeField] private float jumpPower;

    //대쉬
    public bool isDashing = false;
    [SerializeField] private float dashDistance;
    [SerializeField] private float dashTime;

    //물리
    private Rigidbody2D rb;

    //에니메이션
    private bool flip = false;
    private SpriteRenderer playerRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {

        if( isDashing )
        {
            return;
        }
      
        Move();
        Animation();

        if( Input.GetKeyDown( KeyCode.LeftShift) )
        {
            StartCoroutine( Dash() );
        }

    }

    #region 움직임

    void Move()
    {

        //이동
        float moveX = Input.GetAxis("Horizontal") * playerSpeed * Time.deltaTime;

        transform.position = new Vector2( transform.position.x + moveX, transform.position.y );

        //점프
        if( Input.GetKeyDown( KeyCode.W ) && nowJumpCount > 0 )
        {
            rb.AddForce( Vector2.up * jumpPower, ForceMode2D.Impulse );
            nowJumpCount -= 1;
        }

    }

    #endregion

    #region 대쉬

    IEnumerator Dash()
    {

        isDashing = true;

        if(flip)
        {
            rb.velocity = Vector2.left * dashDistance ;
        }
        else
        {
            rb.velocity = Vector2.right * dashDistance ;
        }

        yield return  new WaitForSeconds( dashTime );

        rb.velocity = Vector2.zero ;

        isDashing = false;

    }

    #endregion

    #region 에니메이션

    void Animation()
    {

        switch( Input.GetAxisRaw("Horizontal") )        //방향전환
        {
            case -1:
                flip = true;
                break;
            case 1:
                flip = false;
                break;
        }

        playerRenderer.flipX = flip;

    }

    #endregion

    #region 충돌 관련

    void OnCollisionStay2D( Collision2D collision )
    {

        //점프관련
        if( collision.gameObject.tag == "Ground" )
        {
            nowJumpCount = maxJumpCount;
        }
        
    }

    #endregion

}
