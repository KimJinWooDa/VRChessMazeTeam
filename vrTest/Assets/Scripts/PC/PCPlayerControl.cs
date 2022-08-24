using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCPlayerControl : PlayerControl {
    [Header("Preset Values")]
    public Transform camTransform;
    public LegacyVRRayController[] lcontroller = { null, null }; //left, right

    [Header("Camera Movement")]
    [SerializeField] private float moveSpeed = 10f;
    [Header("Camera Rotation")]
    [SerializeField] private float rotationSpeed = 500;
    [SerializeField] private Vector2 pitchAngleLimit = new Vector2(-30, 60f);

    Vector2 rotation;

    void Start() {
        rotation = camTransform.eulerAngles;
    }

    protected override void LateUpdate() {
        base.LateUpdate();
        UpdateInput();
        RotateCamera();
    }

    public bool Chained() {
        return lcontroller[0].lastShoot || lcontroller[1].lastShoot;
    }

    private void UpdateInput() {
        Vector3 vel = Vector3.zero;
        float s = Chained() ? 0 : 1;//set to 0 when chained

        if (Input.GetKey(KeyCode.D)) {
            vel += RightVector() * moveSpeed;
        }
        if (Input.GetKey(KeyCode.A)) {
            vel += RightVector() * moveSpeed * -1;
        }
        if (Input.GetKey(KeyCode.W)) {
            vel += ForwardVector() * moveSpeed;
        }
        if (Input.GetKey(KeyCode.S)) {
            vel += ForwardVector() * moveSpeed * -1;
        }
        rigid.AddForce(vel * s * Time.deltaTime, ForceMode.VelocityChange);
    }

    private void RotateCamera() {
        rotation.y += Input.GetAxis("Mouse X") * 1 / 60 * rotationSpeed;
        rotation.y = rotation.y % 360;
        rotation.x += -Input.GetAxis("Mouse Y") * 1 / 60 * rotationSpeed;
        rotation.x = Mathf.Clamp(rotation.x, pitchAngleLimit.x, pitchAngleLimit.y);
        camTransform.eulerAngles = rotation;
    }

    private Vector3 ForwardVector() {
        Vector3 v = camTransform.forward;
        v.y = 0;
        v.Normalize();
        return v;
    }

    private Vector3 RightVector() {
        Vector3 v = camTransform.right;
        v.y = 0;
        v.Normalize();
        return v;
    }
}
