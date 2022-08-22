using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZigZagGround : MonoBehaviour
{
    Vector3 originPos;
    [SerializeField] float t;

    float timer = 0f;
    [SerializeField] float time = 3f;

    [SerializeField] float speed = 2f;
    private void Update()
    {
        transform.Translate(t * Vector3.forward * Time.deltaTime * speed);

        if(timer < time)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0f;
            t *= -1f;
        }
    }
}
