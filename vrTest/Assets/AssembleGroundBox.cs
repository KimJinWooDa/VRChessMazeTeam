using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssembleGroundBox : MonoBehaviour
{
    [SerializeField] Vector3 targetMovePosition;

    public bool isAssemble;

    [SerializeField] float speed = 1.5f;
    private void Update()
    {
       if(isAssemble) transform.position = Vector3.MoveTowards(this.transform.position, targetMovePosition, Time.deltaTime * speed);
    }
}
