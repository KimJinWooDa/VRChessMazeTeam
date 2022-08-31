using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReStart : MonoBehaviour
{
    //[SerializeField] Transform reStartPosition;
    [SerializeField] RotateCharacter rc;
    [SerializeField] VRRayController vc;
    [SerializeField] PlayerControl pcon;

    Vector3 reStartPosition;
    private void Awake()
    {
        reStartPosition = transform.position;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SKY"))
        {
            transform.position = reStartPosition;
        }
    }

    public void ReStartGame()
    {
        transform.position = reStartPosition;

        if(GameManager.instance.stageNum == 2)
        {
            rc.StopUp();
            rc.isTriggerState = false;
        }
        foreach (VRRayController vr in FindObjectsOfType<VRRayController>())
        {
            if (vr.lastShoot)
            {
                vr.CompletelyDetach();
            }
        }
        foreach (NewVRController vr2 in FindObjectsOfType<NewVRController>())
        {
            if (vr2.lastShoot)
            {
                vr2.CompletelyDetach();
            }
        }
        if (vc != null)
        { 
        vc.magnet = null;
        vc.canTrigger = false; 
        }

        pcon.canJump = true;
        pcon.isFlying = false;
    }
}
