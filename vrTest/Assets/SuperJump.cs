using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperJump : MonoBehaviour
{
    Vector3 originPos;
    private void Awake()
    {
        originPos = this.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SKY"))
        {
            transform.position = originPos;
        }
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerControl>().jumpPower *= 2f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerControl>().jumpPower = 4f;
        }
    }
}
