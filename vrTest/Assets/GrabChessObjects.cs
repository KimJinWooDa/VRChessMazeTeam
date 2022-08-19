using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabChessObjects : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("BISHOP") && OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            GameManager.instance.SetUI(0, false);
            GameObject.Find("Fade Manager").GetComponent<FadeManager>().GoToScene(1);
        }
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            GameManager.instance.SetUI(0, false);
            GameObject.Find("Fade Manager").GetComponent<FadeManager>().GoToScene(1);
        }
    }
}
