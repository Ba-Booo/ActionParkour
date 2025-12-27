using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Fade : MonoBehaviour
{

    private bool sceneChange = false;
    private Image image;
    private Color color;
    [SerializeField] private Color sceneTransitionColor;
    [SerializeField] private string sceneName;


    void Start()
    {
        image = GetComponent<Image>();
        StartCoroutine( FadeIn() );
    }

    public IEnumerator FadeIn()
    {

        Color color = new Color( 0, 0, 0, 1 );

        for( int i = 0; i < 100; i ++)
        {

            color.a -= 0.01f;
            image.color = color;
            yield return new WaitForSeconds( 0.01f );

        }
        
    }

    public IEnumerator Death()
    {

        if( sceneChange )
        {
            yield break;
        }

        sceneChange = true;

        yield return new WaitForSeconds( 0.5f );

        Color color = new Color( 0, 0, 0, 0 );

        for( int i = 0; i < 100; i ++)
        {
            
            color.a += 0.01f;
            image.color = color;
            yield return new WaitForSeconds( 0.01f );

        }

        SceneManager.LoadScene( sceneName );

    }

    public IEnumerator SceneTransitionFadeOut()
    {

        if( sceneChange )
        {
            yield break;
        }

        sceneChange = true;

        Color color = sceneTransitionColor;

        for( int i = 0; i < 100; i ++)
        {

            color.a += 0.01f;
            image.color = color;
            yield return new WaitForSeconds( 0.01f );

        }

        SceneManager.LoadScene( sceneName );

    }

}