using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReStart : MonoBehaviour
{
    [SerializeField] Transform reStartPosition;
    [SerializeField] RotateCharacter rc;
    [SerializeField] VRRayController vc;
    [SerializeField] PlayerControl pcon;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SKY"))
        {
            transform.position = reStartPosition.position;
        }
    }

    public void ReStartGame()
    {
        transform.position = reStartPosition.position;
        rc.StopUp();
        rc.isTriggerState = false;
        if(vc.lastShoot && vc.magnet != null)
        {
            vc.magnet = null;
            vc.RemoveChain();
            vc.canTrigger = false;
            
        }
        pcon.canJump = true;
        pcon.isFlying = false;
    }
}
