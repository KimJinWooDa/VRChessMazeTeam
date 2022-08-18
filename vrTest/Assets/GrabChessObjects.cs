using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabChessObjects : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("BISHOP") && OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            GameObject.Find("Fade Manager").GetComponent<FadeManager>().GoToScene(1);
        }
    }
}
