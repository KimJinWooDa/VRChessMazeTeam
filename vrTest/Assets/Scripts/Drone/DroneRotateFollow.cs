using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneRotateFollow : MonoBehaviour {
    public float spinSpeed = 90f;

    void Update() {
        transform.rotation *= Quaternion.Euler(0, spinSpeed * Time.deltaTime, 0);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
        if (transform.childCount == 0) return;
        Gizmos.color = Color.yellow;
        Vector3 p = transform.GetChild(0).position;
        p.y = transform.position.y;
        float r = (p - transform.position).magnitude;

        Vector3 c = transform.position;
        c.y = transform.GetChild(0).position.y;
        Gizmos.DrawWireSphere(c, r);
    }
#endif
}
