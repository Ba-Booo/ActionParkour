using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{

    // 화면전환
    [SerializeField] private Fade fade;
    
    // 체력
    public int nowHP;
    [SerializeField] private int maxHP;
    [SerializeField] private ParticleSystem deathParticle;

    // 움직임
    [SerializeField] private float playerSpeed;

    // 점프
    private int nowJumpCount;
    [SerializeField] private int maxJumpCount;
    [SerializeField] private float jumpPower;

    // 대쉬
    [SerializeField] private int dashCount = 1;
    public bool isDashing = false;
    [SerializeField] private float dashDistance;
    [SerializeField] private float dashTime;
    [SerializeField] private ParticleSystem dashnParticle;

    // 소울
    public bool usingSoul = false;
    public float angle;
    [SerializeField] private ParticleSystem soulSummonParticle;
    [SerializeField] private ParticleSystem soulCollectParticle;

    // 마우스
    private Vector2 mouseDistance;
    [SerializeField] private Transform mouseTransform;

    // 물리
    private Rigidbody2D rb;

    // 에니메이션
    private bool flip = false;
    private SpriteRenderer playerRenderer;

    // 충격파
    [SerializeField] private float shockWaveTime;
    [SerializeField] private Renderer shockWave;
    


    #region 센터
    void Awake()
    {
        nowHP = maxHP;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {

        // 쥬금
        if( nowHP <= 0 )
        {
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
            deathParticle.Play();
            StartCoroutine( fade.Death() );
            return;
        }
      
        // 움직임
        Move();
        Animation();

        // 방향 계산
        mouseDistance = mouseTransform.position - transform.position; 
        angle = Mathf.Atan2( mouseDistance.y, mouseDistance.x ) * Mathf.Rad2Deg;

        // 대쉬 횟수 음수 방지
        if( dashCount < 0 )
        {
            dashCount = 0;
        }

        // 스킬
        if( ( Input.GetKeyDown( KeyCode.LeftShift ) || Input.GetMouseButtonDown(0) ) && dashCount >= 1 )
        {
            StartCoroutine( Dash() );
        }

        if( ( Input.GetKeyDown( KeyCode.E ) || Input.GetMouseButtonDown(1) ) && !usingSoul )
        {
            SoulSummon();
            StartCoroutine( ShockWaveSummon( -0.1f, 1f ) );
        }
        else if( ( Input.GetKeyDown( KeyCode.E ) || Input.GetMouseButtonDown(1) ) && usingSoul )
        {
            SoulRecovery();
            StartCoroutine( ShockWaveRecovery( -0.1f, 1f ) );
        }

    }

    #endregion

    #region 움직임

    void Move()
    {

        // 이동
        float moveX = Input.GetAxis("Horizontal") * playerSpeed * Time.deltaTime;

        transform.position = new Vector2( transform.position.x + moveX, transform.position.y );

        // 점프
        if( ( Input.GetKeyDown( KeyCode.W ) || Input.GetKeyDown( KeyCode.UpArrow ) || Input.GetKeyDown( KeyCode.Space ) )&& nowJumpCount > 0 )
        {
            rb.AddForce( Vector2.up * jumpPower, ForceMode2D.Impulse );
            nowJumpCount -= 1;
        }

    }

    #endregion

    #region 대쉬

    IEnumerator Dash()
    {

        dashnParticle.Play();

        isDashing = true;

        mouseDistance = mouseTransform.position - transform.position; 
        
        int layerMask = 1 << LayerMask.NameToLayer("Ground");                 //땅 관련
        RaycastHit2D hit = Physics2D.Raycast( transform.position, mouseDistance.normalized, dashDistance, layerMask);
        Debug.DrawRay(transform.position, mouseDistance.normalized  * dashDistance * 10, Color.red);

        if( hit )
        {
            rb.velocity = mouseDistance.normalized * Vector2.Distance( transform.position, hit.point ) * dashDistance;
        }
        else
        {
            rb.velocity = mouseDistance.normalized * dashDistance * 10f;
        }
        
        yield return  new WaitForSeconds( dashTime );

        rb.velocity = Vector2.zero ;

        dashCount -= 1;
        isDashing = false;

    }

    #endregion

    #region 영혼

    void SoulSummon()
    {
        usingSoul = true;
        soulSummonParticle.Play();
    }

    void SoulRecovery()
    {
        usingSoul = false;
        soulCollectParticle.Play();
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

    IEnumerator ShockWaveSummon( float startPosition, float endPosition )
    {

        shockWave.material.SetFloat( "_waveDistance", startPosition );

        float elapsedTime = 0;
        while ( elapsedTime <= shockWaveTime)
        {
            elapsedTime += Time.deltaTime * 2;
            float lerpedAmount = Mathf.Lerp( startPosition, endPosition, elapsedTime / shockWaveTime );
            shockWave.material.SetFloat( "_waveDistance", elapsedTime );
            yield return null;
        }

    }

    IEnumerator ShockWaveRecovery( float startPosition, float endPosition )
    {

        shockWave.material.SetFloat( "_waveDistance", endPosition );

        float elapsedTime = shockWaveTime;
        while ( elapsedTime >= startPosition )
        {
            elapsedTime -= Time.deltaTime;
            float lerpedAmount = Mathf.Lerp( endPosition, startPosition, elapsedTime / shockWaveTime );
            shockWave.material.SetFloat( "_waveDistance", elapsedTime );
            yield return null;
        }

    }

    #endregion



    #region 충돌 관련

    void OnCollisionEnter2D( Collision2D collision )
    {

        // 점프관련
        if( collision.gameObject.tag == "Ground" )
        {
            nowJumpCount = maxJumpCount;
        }
        
    }

    void OnCollisionStay2D( Collision2D collision )
    {

        // 대쉬관련
        if( collision.gameObject.tag == "Ground" )
        {
            dashCount = 1;
        }
        
    }

    void OnTriggerEnter2D( Collider2D collider )
    {

        // 대쉬관련
        if( collider.gameObject.tag == "Soul" && usingSoul )
        {
            dashCount += 1;
        }
        
        // 대쉬관련
        if( collider.gameObject.tag == "ExtraSoul" )
        {
            dashCount += 1;
        }

        if( collider.gameObject.tag == "Obstacle" )
        {
            nowHP -= 1;
        }

        if( collider.gameObject.tag == "SceneTransition" )
        {
            StartCoroutine( fade.SceneTransitionFadeOut() );
        }
        
    }

    void OnTriggerExit2D( Collider2D collider )
    {

        // 대쉬관련
        if( collider.gameObject.tag == "Soul" && usingSoul )
        {
            dashCount -= 1;
        }
        if( collider.gameObject.tag == "ExtraSoul" )
        {
            dashCount -= 1;
        }
        
    }

    #endregion

}
