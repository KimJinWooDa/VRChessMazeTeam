using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCharacter : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;

    [SerializeField] Camera cameraToLookAt;
    void Start()
    {
        //cameraToLookAt = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }


    // Update is called once per frame

    void Update()
    {
        transform.LookAt(transform.position + cameraToLookAt.transform.rotation * Vector3.back, cameraToLookAt.transform.rotation * Vector3.down);
        transform.position = target.position + offset;
    }
}
