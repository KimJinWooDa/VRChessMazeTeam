using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabChessObjects : MonoBehaviour
{
    bool isOnce = false;
    bool isEnter;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("BISHOP") && OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) && !isOnce)
        {
            if (isEnter)
            {
                StartCoroutine(DissolveBishop(other.transform));
                //GameManager.instance.OnCourtineFade(3);

                GameObject.Find("Fade Manager").GetComponent<FadeManager>().GoToScene(1);
                isOnce = true;
            }

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BISHOP"))
        {
            isEnter = false;
        }
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            GameObject.Find("Fade Manager").GetComponent<FadeManager>().GoToScene(1);
        }
    }

    IEnumerator DissolveBishop(Transform bishop)
    {
        float value = bishop.GetComponent<Renderer>().material.GetFloat("_Alpha");
        while (value < 1)
        {
            float t = 0;
            t += 0.3f * Time.deltaTime;
            bishop.GetComponent<Renderer>().material.SetFloat("_Alpha", t);
            yield return null;
        }
    }
}
