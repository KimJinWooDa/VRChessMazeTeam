using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCharacter : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;

    [SerializeField] float offsetZ = 1f;
    // Update is called once per frame

    void Update()
    {
        //Vector3 targetPos = new Vector3(target.position.x, target.position.y, offsetZ);
        //transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 2f);
    }
}
