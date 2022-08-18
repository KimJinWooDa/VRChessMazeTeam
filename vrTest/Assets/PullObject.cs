using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullObject : MonoBehaviour
{
    public float pullSpeed;

    public bool canPull;

    public bool testPull;

    Rigidbody rb;

    public VRRayController target;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void CanPull()
    {
        canPull = true;
    }

    public void Update()
    {
        if (testPull || canPull)
        {
            Vector3 dir = target.transform.position - transform.position;
            rb.MovePosition(rb.position + dir * pullSpeed * Time.deltaTime);

        }

    }
}
