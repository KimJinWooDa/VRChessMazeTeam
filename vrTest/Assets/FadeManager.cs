using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class FadeManager : MonoBehaviour
{
    public FadeScreen fadeScreen;

    public void GoToScene(int index)
    {
        StartCoroutine(ChangeScene(index));
    }

    IEnumerator ChangeScene(int index)
    {
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(fadeScreen.fadeTime);

        SceneManager.LoadScene(index);
    }
}
