using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallSky : MonoBehaviour
{
    [SerializeField] Transform reStartPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SKY"))
        {
            transform.position = reStartPosition.position;
        }
    }
}
