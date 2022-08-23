using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YeetMagnet : Magnet {
    public static Rigidbody prigid;
    public static VRRayController[] vrrc;
    public float power = 10f;

    public override void Awake() {
        base.Awake();
        if(prigid == null) prigid = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        if (vrrc == null || vrrc.Length == 0) vrrc = GameObject.FindObjectsOfType<VRRayController>();
    }

    public override void Open() {
        Vector3 tv = transform.forward * power;

        //unchain everything
        foreach (VRRayController vr in vrrc) {
            if(vr.lastShoot) vr.Detach();
        }

        //yeet
        prigid.velocity = tv;
    }
}
