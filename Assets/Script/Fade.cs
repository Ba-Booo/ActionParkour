using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{

    private Image image;
    [SerializeField] private PlayerControl playerControl;

    void Start()
    {
        image = GetComponent<Image>();
        StartCoroutine( FadeIn() );
    }

    void Update()
    {

        if( playerControl.nowHP <= 0 )
        {
            StartCoroutine( Death() );
        }

    }

    public IEnumerator FadeIn()
    {

        Color color = image.color;

        for( int i = 0; i < 100; i ++)
        {

            color.a -= 0.01f;
            image.color = color;
            yield return new WaitForSeconds( 0.01f );

        }
        
    }

    public IEnumerator Death()
    {

        yield return new WaitForSeconds( 0.5f );

        Color color = image.color;

        for( int i = 0; i < 100; i ++)
        {
            
            color.a += 0.01f;
            image.color = color;
            yield return new WaitForSeconds( 0.01f );

        }

        

    }

}
