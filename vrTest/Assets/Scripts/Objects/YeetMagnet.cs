using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YeetMagnet : Magnet {
    //@JUNWOO edit this if the magnets lack power in general
    private const float POWER_MULT = 1f;

    public static Rigidbody prigid;
    public static VRRayController[] vrrc;
    public float power;

    public override void Awake() {
        base.Awake();

        if (prigid == null) prigid = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        if (vrrc == null || vrrc.Length == 0) vrrc = GameObject.FindObjectsOfType<VRRayController>();
    }

    public override void Open() {
        Vector3 tv = transform.forward * power * POWER_MULT;

        foreach (VRRayController vr in vrrc) {
            if (vr.lastShoot)
            {
                vr.Detach();

            }
        }

        //yeet
        prigid.velocity = tv;
        StartCoroutine(IReset(1));
    }

    IEnumerator IReset(float duration) {
        yield return new WaitForSeconds(duration);
        opened = false;
    }
}
