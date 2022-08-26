using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearStage : MonoBehaviour
{
    Vector3 originPos;
    Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();   
        originPos = transform.position;

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GOALIN"))
        {
            GameObject fadeManager = GameObject.Find("Fade Manager");
            GameManager.instance.stageNum++;
            fadeManager.GetComponent<FadeManager>().GoToScene(0);

            if(this.gameObject != null) Destroy(this.gameObject);
        }

        if (other.CompareTag("SKY"))
        {
            rb.useGravity = false;
            transform.position = originPos;
        }
    }
}
