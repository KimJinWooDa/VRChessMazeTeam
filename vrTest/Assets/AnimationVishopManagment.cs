using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationVishopManagment : MonoBehaviour
{
    [SerializeField] GameObject[] animationVishops;
    int count;
    private void Awake()
    {
        count = animationVishops.Length;
        StartCoroutine(StartAnimation());
    }

    IEnumerator StartAnimation()
    {
        int counting = 0;
        while(counting < count)
        {
            animationVishops[counting].GetComponent<RotateVishop>().isStart = true;
            counting++;
            yield return new WaitForSeconds(0.3f);
        }

        yield return new WaitForSeconds(5f);
        StartCoroutine(StartAnimation());
    }
}
