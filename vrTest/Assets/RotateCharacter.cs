using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCharacter : MonoBehaviour
{
    public bool isTriggerState;
    Transform targetMagnet;

    [SerializeField] float angle = 90f;
    [SerializeField] float radius = 1f;

    [SerializeField] VRRayController vrRayController;

    Quaternion originRotation;
    [SerializeField] Transform myCharacter;
    private void Awake()
    {
        originRotation = this.transform.rotation;
    }
    private void Update()
    {
        if (isTriggerState)
        {
            transform.RotateAround(targetMagnet.position, Vector3.up  * radius, angle);
        }
    }

    public void Set(Transform targetMagnet, bool isOn)
    {
        isTriggerState = isOn;
        this.targetMagnet = targetMagnet;
        StartCoroutine(UpPosition());
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("GROUND") && (!OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger)))
        {
            if(vrRayController.chain !=null) vrRayController.RemoveChain();
            targetMagnet = null;
        }
    }
    IEnumerator UpPosition()
    {
        if (myCharacter.transform.position.y < targetMagnet.position.y)
        {
            while (myCharacter.transform.position.y < targetMagnet.position.y && isTriggerState)
            {
                myCharacter.transform.position = new Vector3(myCharacter.transform.position.x, myCharacter.transform.position.y + Time.deltaTime * 1f, myCharacter.transform.position.z - Time.deltaTime * 1f);
                yield return null;
            }
        }
      else
        {
            float offsetY = transform.position.y - targetMagnet.position.y;
            while (myCharacter.transform.position.y > myCharacter.transform.position.y - offsetY && isTriggerState)
            {
                myCharacter.transform.position = new Vector3(myCharacter.transform.position.x, myCharacter.transform.position.y - Time.deltaTime * 1f, myCharacter.transform.position.z + Time.deltaTime * 1f);
                yield return null;
            }
        }
    }
}
