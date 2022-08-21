using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetWeapon : MonoBehaviour
{
    public bool getWeapon;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("WEAPON"))
        {
            getWeapon = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("WEAPON"))
        {
            getWeapon = false;
        }
    }
}
