using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabChessObjects : MonoBehaviour
{
    bool isOnce = false;
    bool isEnter;
    private void Start()
    {
        isOnce = false;
        isEnter = false;
    }
    private void OnTriggerStay(Collider other)
    {

        if (other.CompareTag("ROCK"))
        {
            isEnter = true;
            if (isEnter && OVRInput.Get(OVRInput.Button.SecondaryHandTrigger) && !isOnce ) //&& GameManager.instance.stageNum == 1
            {
                if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
                {
                    //StartCoroutine(DissolveChess(other.transform));
                    other.GetComponent<ChessState>().TriggerState();
                    GameManager.instance.stageNum = 1;
                    GameObject.Find("Fade Manager").GetComponent<FadeManager>().GoToScene(1);
                    isOnce = true;
                }

            }

        }
        if (other.CompareTag("BISHOP"))
        {
            isEnter = true;
            if (isEnter && OVRInput.Get(OVRInput.Button.SecondaryHandTrigger) && !isOnce) // && GameManager.instance.stageNum == 2
            {
                if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
                {
                    //StartCoroutine(DissolveChess(other.transform));
                    other.GetComponent<ChessState>().TriggerState();
                    GameManager.instance.stageNum = 2;
                    GameObject.Find("Fade Manager").GetComponent<FadeManager>().GoToScene(2);
                    isOnce = true;
                }
            }

        }
        if (other.CompareTag("KNIGHT"))
        {
            isEnter = true;
            if (isEnter && OVRInput.Get(OVRInput.Button.SecondaryHandTrigger) && !isOnce) // && GameManager.instance.stageNum == 3
            {
                //StartCoroutine(DissolveChess(other.transform));
                if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
                {
                    other.GetComponent<ChessState>().TriggerState();
                    GameManager.instance.stageNum = 3;
                    GameObject.Find("Fade Manager").GetComponent<FadeManager>().GoToScene(3);
                    isOnce = true;
                }

            }

        }
        if (other.CompareTag("PAWN"))
        {
            isEnter = true;
            if (isEnter && OVRInput.Get(OVRInput.Button.SecondaryHandTrigger) && !isOnce) // && GameManager.instance.stageNum == 4
            {
                //StartCoroutine(DissolveChess(other.transform));
                if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
                {
                    other.GetComponent<ChessState>().TriggerState();
                    GameManager.instance.stageNum = 4;
                    GameObject.Find("Fade Manager").GetComponent<FadeManager>().GoToScene(4);
                    isOnce = true;
                }

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
        if (other.CompareTag("PAWN"))
        {
            isEnter = false;
        }
    }

   // private void Update()
   // {
        //if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        //{
        //    GameManager.instance.stageNum = 2;
        //    GameObject.Find("Fade Manager").GetComponent<FadeManager>().GoToScene(2);
        //}
        //if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger))
        //{
        //    GameManager.instance.stageNum = 1;
        //    GameObject.Find("Fade Manager").GetComponent<FadeManager>().GoToScene(1);
        //}
        //if (OVRInput.GetDown(OVRInput.Button.Three))
        //{
        //    GameManager.instance.stageNum = 3;
        //    GameObject.Find("Fade Manager").GetComponent<FadeManager>().GoToScene(3);
        //}
        //if (OVRInput.GetDown(OVRInput.Button.Four))
        //{
        //    GameManager.instance.stageNum = 4;
        //    GameObject.Find("Fade Manager").GetComponent<FadeManager>().GoToScene(4);
        //}
    //}

    //IEnumerator DissolveChess(Transform obj)
    //{
    //    float value = obj.GetComponent<Renderer>().material.GetFloat("_Alpha");
    //    while (value < 1)
    //    {
    //        float t = 0;
    //        t += 0.35f * Time.deltaTime;
    //        obj.GetComponent<Renderer>().material.SetFloat("_Alpha", t);
    //        yield return null;
    //    }
    //}
}
