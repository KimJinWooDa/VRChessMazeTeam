using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckState : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WARNING") && GameManager.instance.stageNum < 5)
        {
            GameManager.instance.SetUI(true);
            GameManager.instance.isPingPong = true;
            GameManager.instance.StartPingPong();
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("WARNING") && GameManager.instance.stageNum < 5)
        {
            GameManager.instance.SetUI(false);
            GameManager.instance.isPingPong = false;
        }
    }

}
