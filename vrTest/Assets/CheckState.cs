using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckState : MonoBehaviour
{
   
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WARNING"))
        {
            GameManager.instance.SetUI(true);
            GameManager.instance.isPingPong = true;
            GameManager.instance.StartPingPong();

        }
        if (other.CompareTag("CHESSBOARD"))
        {
            GameManager.instance.OnCourtineFade();
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("WARNING"))
        {
            GameManager.instance.SetUI(false);
            GameManager.instance.isPingPong = false;
        }
    }

}
