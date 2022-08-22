using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabChessObjects : MonoBehaviour
{
    bool isOnce = false;
    bool isEnter;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("BISHOP"))
        {
            isEnter = true;
            if (isEnter && OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) && !isOnce && GameManager.instance.stageNum == 2)
            {
                StartCoroutine(DissolveChess(other.transform));

                GameObject.Find("Fade Manager").GetComponent<FadeManager>().GoToScene(2);
                isOnce = true;
            }

        }
        if (other.CompareTag("ROCK"))
        {
            isEnter = true;
            if (isEnter && OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) && !isOnce && GameManager.instance.stageNum == 1)
            {
                StartCoroutine(DissolveChess(other.transform));

                GameObject.Find("Fade Manager").GetComponent<FadeManager>().GoToScene(1);
                isOnce = true;
            }

        }
        if (other.CompareTag("KNIGHT"))
        {
            isEnter = true;
            if (isEnter && OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) && !isOnce && GameManager.instance.stageNum == 3)
            {
                StartCoroutine(DissolveChess(other.transform));

                GameObject.Find("Fade Manager").GetComponent<FadeManager>().GoToScene(3);
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
        if (other.CompareTag("ROCK"))
        {
            isEnter = false;
        }
        if (other.CompareTag("KNIGHT"))
        {
            isEnter = false;
        }
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            GameManager.instance.stageNum = 2;
            GameObject.Find("Fade Manager").GetComponent<FadeManager>().GoToScene(2);
        }
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger))
        {
            GameManager.instance.stageNum = 1;
            GameObject.Find("Fade Manager").GetComponent<FadeManager>().GoToScene(1);
        }
    }

    IEnumerator DissolveChess(Transform obj)
    {
        float value = obj.GetComponent<Renderer>().material.GetFloat("_Alpha");
        while (value < 1)
        {
            float t = 0;
            t += 1.2f * Time.deltaTime;
            obj.GetComponent<Renderer>().material.SetFloat("_Alpha", t);
            yield return null;
        }
    }
}
