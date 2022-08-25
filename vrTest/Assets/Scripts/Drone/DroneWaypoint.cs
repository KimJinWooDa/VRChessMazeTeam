using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneWaypoint : MonoBehaviour
{
#if UNITY_EDITOR
    private void OnDrawGizmos() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position, 0.1f);
        Gizmos.DrawLine(transform.position, transform.position + transform.forward);
    }
#endif
}
