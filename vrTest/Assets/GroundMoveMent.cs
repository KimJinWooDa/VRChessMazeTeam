using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMoveMent : MonoBehaviour
{
    public bool canMoving;
    [SerializeField] float moveSpeed = 1f;
    Vector3 originPos;
    [SerializeField] Transform targetPos;
    private void Awake()
    {
        originPos = this.transform.position;
    }

    void Update()
    {
        if (canMoving && !isCrash) {
            transform.position = Vector3.Lerp(transform.position, targetPos.position, moveSpeed * Time.deltaTime);
        }
        else if(canMoving && isCrash)
        {
            transform.position = this.transform.position;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, originPos, moveSpeed * Time.deltaTime);
        }
    }

    public void CanMove(bool isOn)
    {
        canMoving = isOn;
    }

    bool isCrash;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("GROUND"))
        {
            isCrash = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("GROUND"))
        {
            isCrash = false;
        }
    }
}
