using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static VRRayController;

public class PCControllerFrame : MonoBehaviour
{
    [Header("Preset Values")]
    public PCPlayerControl pcon;
    public Transform camTransform;

    [Header("Controls")]
    public float defaultAimLength = 10f;

    private Camera cam;
    private bool[] justDetached = { false, false };

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        cam = camTransform.GetComponent<Camera>();
    }

    private void Update() {
        //lerp hands
        transform.rotation = Quaternion.Lerp(transform.rotation, camTransform.rotation, Time.deltaTime * 5f);

        UpdateInput(0);
        UpdateInput(1);
    }

    private void UpdateInput(int id) {
        LegacyVRRayController con = pcon.lcontroller[id];
        con.overrideTarget = true;
        Quaternion aim = transform.rotation;

        //todo control
        if (con.lastShoot) {
            if (Input.GetMouseButtonDown(id)) {
                //detach
                con.testGrip = con.testTrigger = false;
                aim = transform.rotation;
                justDetached[id] = true;
            }
            else {
                aim = Quaternion.LookRotation(con.magnet.transform.position - con.transform.position);
            }
        }
        else {
            if (justDetached[id]) {
                if(!Input.GetMouseButton(id) && !Input.GetMouseButtonUp(id)) justDetached[id] = false;
            }
            else {
                if (Input.GetMouseButton(id) || Input.GetMouseButtonUp(id)) {
                    con.testGrip = true;
                    con.testTrigger = false;
                    //aim
                    Ray ray = new Ray(camTransform.position, camTransform.forward);
                    if (Physics.Raycast(ray, out RaycastHit rinfo, MAGNET_RANGE, 1 << MAGNET_LAYER)) {
                        aim = Quaternion.LookRotation(rinfo.collider.transform.position - con.transform.position);
                    }
                    else {
                        aim = Quaternion.LookRotation(camTransform.position + camTransform.forward * defaultAimLength - con.transform.position);
                    }
                }
                else {
                    con.testGrip = con.testTrigger = false;
                }

                if (Input.GetMouseButtonUp(id) && con.hovering) {
                    //try shooting
                    Debug.Log("Try shoot");
                    con.testGrip = con.testTrigger = true;
                }
            }
        }

        con.overrideRotation = aim;
    }
}
