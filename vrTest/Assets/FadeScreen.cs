using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScreen : MonoBehaviour
{
    public float fadeTime = 2f;
    public Color fadeColor = Color.white;
    public bool isFadeStart = true;
    Renderer renderer;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        if (isFadeStart)
        {
            Color color = new Vector4(1, 1, 1, 0);
            renderer.material.SetColor("_Color", color);
        }
    }

    public void FadeIn()
    {
        Fade(1, 0);
    }

    public void FadeOut()
    {
        Fade(0, 1);
    }

    public void Fade(float alphaFadeIn, float alphaFadeOut)
    {
        StartCoroutine(IFadeRoutine(alphaFadeIn, alphaFadeOut));
    }

    IEnumerator IFadeRoutine(float alphaFadeIn, float alphaFadeOut)
    {
        float timer = 0;
        while (timer <= fadeTime)
        {
            Color color = fadeColor;
            color.a = Mathf.Lerp(alphaFadeIn, alphaFadeOut, timer / fadeTime);
            renderer.material.SetColor("_Color", color);
            timer += Time.deltaTime;
            yield return null;
        }
        Color color2 = fadeColor;
        color2.a = alphaFadeOut;
        renderer.material.SetColor("_Color", color2);
    }
}
