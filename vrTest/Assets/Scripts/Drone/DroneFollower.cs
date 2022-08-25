using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneFollower : MonoBehaviour {
    public Transform waypointParent;
    public float moveDuration = 3f;

    private int cami;
    private float time = 0f;

    void Start() {
        transform.position = waypointParent.GetChild(0).position;
        transform.rotation = waypointParent.GetChild(0).rotation;
        cami = 0;
        time = 0f;
    }

    void Update() {
        if (cami >= waypointParent.childCount - 1) return;

        time += Time.deltaTime;
        if (time >= (cami + 1) * moveDuration) cami++;
        if (cami >= waypointParent.childCount - 1) return;
        transform.position = Vector3.Lerp(waypointParent.GetChild(cami).position, waypointParent.GetChild(cami + 1).position, (time - cami * moveDuration) / moveDuration);
        transform.rotation = Quaternion.Lerp(waypointParent.GetChild(cami).rotation, waypointParent.GetChild(cami + 1).rotation, (time - cami * moveDuration) / moveDuration);
    }

    //v = Lerp(v, end, speed * delta)
    //v = Lerp(start, end, f); f += speed * delta

#if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
        if (waypointParent == null) return;
        Gizmos.color = Color.yellow;
        int n = waypointParent.childCount;

        if(n <= 1) return;
        for (int i = 0; i < n - 1; i++) {
            Gizmos.DrawLine(waypointParent.GetChild(i).position, waypointParent.GetChild(i + 1).position);
        }
    }
#endif
}
