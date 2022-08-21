using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetWeapon : MonoBehaviour
{
    public bool getWeapon;
    [SerializeField] Transform swordPos;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("WEAPON") && OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            other.transform.SetParent(swordPos);
            other.GetComponent<Enemy>().StartAttack(true);
        }
        else if(other.CompareTag("WEAPON") && OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
        {
            swordPos.DetachChildren();
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("WEAPON"))
        {
            other.GetComponent<Enemy>().StartAttack(false);
        }
    }
}
