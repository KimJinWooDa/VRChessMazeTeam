using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCharacter : MonoBehaviour
{
    public bool isTriggerState;
    [SerializeField] public Transform targetMagnet;

    [SerializeField] float angle;
    [SerializeField] float radius = 1f;

    [SerializeField] VRRayController vrRayController;


    [SerializeField] Transform myCharacter;

    private void Update()
    {
        if (isTriggerState)
        {
            transform.RotateAround(targetMagnet.position, Vector3.up, angle * Time.deltaTime);
        }

        if(isTriggerState && angle < 100f)
        {
            angle += Time.deltaTime * 10f;
        }

    }
    public void StopUp()
    {
        angle = 20f;
        myCharacter.GetComponent<Rigidbody>().isKinematic = false;
        StopCoroutine(UpPosition());
    }
    public void Set(Transform targetMagnet, bool isOn)
    {
        isTriggerState = isOn;
        this.targetMagnet = targetMagnet;
        StartCoroutine(UpPosition());
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("GROUND"))
        {
            myCharacter.GetComponent<PlayerControl>().isFlying = false;
        }
    }

    public IEnumerator UpPosition()
    {
        if (myCharacter.transform.position.y < targetMagnet.position.y)
        {
            while (myCharacter.transform.position.y < targetMagnet.position.y && isTriggerState)
            {
                myCharacter.transform.position = new Vector3(myCharacter.transform.position.x, myCharacter.transform.position.y + Time.deltaTime * 1f, myCharacter.transform.position.z - Time.deltaTime * 0.75f);
                yield return null;
            }
        }
        else
        {
            while (myCharacter.position.y > targetMagnet.position.y && isTriggerState)
            {
                myCharacter.transform.position = new Vector3(myCharacter.transform.position.x, myCharacter.transform.position.y - Time.deltaTime * 0.8f, myCharacter.transform.position.z + Time.deltaTime * 1f);
                yield return null;
            }
        }
    }
}
