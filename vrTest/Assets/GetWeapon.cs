using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetWeapon : MonoBehaviour
{
    public bool getWeapon;
    [SerializeField] Transform swordPos;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("WEAPON") && OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {
            other.transform.SetParent(swordPos);
            other.GetComponent<Enemy>().StartAttack(true);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("WEAPON"))
        {
            swordPos.DetachChildren();
            other.GetComponent<Enemy>().StartAttack(false);
        }
    }
}
