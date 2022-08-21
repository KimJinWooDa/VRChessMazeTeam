using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabChessObjects : MonoBehaviour
{
    bool isOnce = false;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("BISHOP") && OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger) && !isOnce)
        {
            GameManager.instance.OnCourtineFade(3);
            isOnce = true;
        }
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            GameObject.Find("Fade Manager").GetComponent<FadeManager>().GoToScene(1);
        }

        if (Input.GetKeyDown(KeyCode.Tab) &&!isOnce)
        {
            GameManager.instance.OnCourtineFade(3);
            isOnce = true;
        }
    }
}
