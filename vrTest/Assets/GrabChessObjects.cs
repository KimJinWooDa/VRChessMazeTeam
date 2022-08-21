using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabChessObjects : MonoBehaviour
{
    bool isOnce = false;
    bool isEnter;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("BISHOP") && OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger) && !isOnce)
        {
            if (isEnter)
            {
                StartCoroutine(DissolveBishop(other.transform));
                GameManager.instance.OnCourtineFade(3);
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

        //if (Input.GetKeyDown(KeyCode.Tab) &&!isOnce)
        //{
        //    GameManager.instance.OnCourtineFade(3);
        //    isOnce = true;
        //}
    }

    IEnumerator DissolveBishop(Transform bishop)
    {
        float value = bishop.GetComponent<Renderer>().material.GetFloat("_Alpha");
        while (value < 1)
        {
            bishop.GetComponent<Renderer>().material.SetFloat("_Alpha", 0.3f * Time.deltaTime);
            yield return null;
        }
    }
}
