using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckState : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WARNING"))
        {
            GameManager.instance.SetUI(1, true);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("WARNING"))
        {
            GameManager.instance.SetUI(1, false);
        }
    }
}
