using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour {
    public const int LEFT = 0, RIGHT = 1;
    private const float FLY_DRAG = 0.9f;

    public bool lastWhite, lastColorExists;
    public bool isStatic;

    public VRRayController[] controller = { null, null }; //left, right
    public Rigidbody rigid;

    bool canJump;
    private void Update()
    {
        if(OVRInput.Get(OVRInput.Button.One) && canJump)  
        {
            rigid.AddForce(Vector3.up * 2f, ForceMode.Impulse);
            canJump = false;
        }
    }

    private void LateUpdate() {
        UpdateFlying();
    }


    private void UpdateFlying()
    {
        rigid.useGravity = !Chained();
    }

    public bool Chained()
    {
        return controller[0].lastShoot || controller[1].lastShoot;
    }

    public void WrongColor() {
        //display help: wrong color
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("GROUND"))
        {
            canJump = true;
        }
    }
}
