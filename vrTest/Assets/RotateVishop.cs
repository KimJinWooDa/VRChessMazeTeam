using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateVishop : MonoBehaviour
{
    Vector3 originPosition;
    Transform pivotPosition;

    public bool isStart;
    private void Awake()
    {
        originPosition = this.transform.position;
        pivotPosition = transform.GetChild(0).transform;
    }

    private void Update()
    {
        if (isStart)
        {
            transform.RotateAround(pivotPosition.position, Vector3.forward, 3f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("STOP"))
        {
            isStart = false;
            transform.position = originPosition;
            transform.rotation = Quaternion.Euler(new Vector3(90f, 0, 0));
        }
    }
}
