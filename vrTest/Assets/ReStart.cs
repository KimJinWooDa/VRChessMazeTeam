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
        //rc.stageOneRotate = false;
        transform.position = reStartPosition;
        //rc.StopUp();
        //rc.isTriggerState = false;
        foreach (VRRayController vr in FindObjectsOfType<VRRayController>())
        {
            if (vr.lastShoot)
            {
                vr.CompletelyDetach();

            }
        }
        vc.magnet = null;
        vc.canTrigger = false;

        pcon.canJump = true;
        pcon.isFlying = false;
    }
}
