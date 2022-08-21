using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    protected enum State { Ä®, ÃÑ };
    protected State state;

    Vector3 originPosition;

    public virtual void Awake()
    {
        originPosition = this.transform.position;
    }


    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SKY"))
        {
            this.transform.position = originPosition;
        }
    }
}
