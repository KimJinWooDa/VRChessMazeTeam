using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YeetMagnet : Magnet {
    //@JUNWOO edit this if the magnets lack power in general
    private const float POWER_MULT = 1f;

    public static Rigidbody prigid;
    public static VRRayController[] vrrc;
    public NewVRController[] nvrrc;
    public float power;
    [SerializeField] private AudioSource audios;

    public override void Awake() {
        base.Awake();

        if (prigid == null) prigid = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        if (vrrc == null || vrrc.Length == 0) vrrc = GameObject.FindObjectsOfType<VRRayController>();
        if(nvrrc == null) nvrrc = GameObject.FindObjectsOfType<NewVRController>();
        if (audios == null) audios = GetComponent<AudioSource>();
    }

    public override void Open() {
        Vector3 tv = transform.forward * power * POWER_MULT;

        if(vrrc.Length != 0)
        {
            foreach (VRRayController vr in vrrc)
            {
                if (vr.lastShoot)
                {
                    vr.CompletelyDetach();
                }
            }
        }
        if (nvrrc.Length != 0)
        {
            foreach (NewVRController vr2 in nvrrc)
            {
                if (vr2.lastShoot)
                {
                    vr2.CompletelyDetach();
                }
            }
        }
  

        //yeet
        prigid.velocity = tv;
        audios.Play();
        StartCoroutine(IReset(1));
    }

    IEnumerator IReset(float duration) {
        yield return new WaitForSeconds(duration);
        opened = false;
    }
}
