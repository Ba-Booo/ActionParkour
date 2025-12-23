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
    [SerializeField] private int dashCount = 1;
    public bool isDashing = false;
    [SerializeField] private float dashDistance;
    [SerializeField] private float dashTime;

    //소울
    private bool usingSoul = false;
    [SerializeField] private GameObject soulPrefab;
    [SerializeField] private ParticleSystem collectParticle;

    //마우스
    private Vector2 mouseDistance;
    [SerializeField] private Transform mouseTransform;

    //물리
    private Rigidbody2D rb;

    //에니메이션
    private bool flip = false;
    private SpriteRenderer playerRenderer;

    //충격파
    [SerializeField] private float shockWaveTime;
    [SerializeField] private Renderer shockWave;


    #region 센터

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

        if( Input.GetKeyDown( KeyCode.LeftShift) && dashCount >= 1 )
        {
            StartCoroutine( Dash() );
        }

        if( Input.GetKeyDown( KeyCode.E ) && !usingSoul)
        {
            SoulSummon();
            StartCoroutine( ShockWaveSummon( -0.1f, 1f ) );
        }
        else if( Input.GetKeyDown( KeyCode.E ) && usingSoul)
        {
            SoulRecovery();
            //StartCoroutine( ShockWaveRecovery( -0.1f, 1f ) );
        }

    }

    #endregion

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

        mouseDistance = mouseTransform.position - transform.position;  
        rb.velocity = mouseDistance.normalized * dashDistance;
        
        yield return  new WaitForSeconds( dashTime );

        rb.velocity = Vector2.zero ;

        dashCount -= 1;
        isDashing = false;

    }

    #endregion

    #region 영혼

    void SoulSummon()
    {
        mouseDistance = mouseTransform.position - transform.position; 
        float angle = Mathf.Atan2( mouseDistance.y, mouseDistance.x ) * Mathf.Rad2Deg;
        GameObject soul = Instantiate( soulPrefab, transform.position, Quaternion.Euler( 0, 0, angle - 90 ) );
        usingSoul = true;
    }

    void SoulRecovery()
    {
        collectParticle.Play();
        usingSoul = false;
    }

    #endregion

    #region 에니메이션 및 임팩트

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

    IEnumerator ShockWaveSummon( float startPosition, float endPosition )     //보류
    {

        shockWave.material.SetFloat( "_waveDistance", startPosition );

        float elapsedTime = 0;
        while ( elapsedTime <= shockWaveTime)
        {
            elapsedTime += Time.deltaTime * 2;
            shockWave.material.SetFloat( "_waveDistance", elapsedTime );
            yield return null;
        }

    }

    // IEnumerator ShockWaveRecovery( float startPosition, float endPosition )     //보류
    // {

    //     shockWave.material.SetFloat( "_waveDistance", startPosition );

    //     float elapsedTime = shockWaveTime;
    //     while ( elapsedTime <= shockWaveTime)
    //     {
    //         elapsedTime -= Time.deltaTime * 2;
    //         shockWave.material.SetFloat( "_waveDistance", elapsedTime );
    //         yield return null;
    //     }

    // }

    #endregion

    #region 충돌 관련

    void OnCollisionStay2D( Collision2D collision )
    {

        //점프관련
        if( collision.gameObject.tag == "Ground" )
        {
            nowJumpCount = maxJumpCount;
            dashCount = 1;
        }
        
    }

    void OnTriggerEnter2D( Collider2D collider )
    {

        //점프관련
        if( collider.gameObject.tag == "Soul" )
        {
            dashCount = 1;
        }
        
    }

    void OnTriggerStay2D( Collider2D collider )
    {

        //점프관련
        if( collider.gameObject.tag == "Soul" )
        {
            dashCount = 1;
        }
        
    }

    #endregion

}
