using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DroneCameraCreator {
    [MenuItem("Tools/Create Drone Waypoint")]
    public static void CreateWaypoint() {
        GameObject parent = GameObject.Find("DroneWays");
        if(parent == null) {
            parent = new GameObject("DroneWays");
        }
        Camera cam = SceneView.lastActiveSceneView.camera;
        GameObject way = new GameObject("Waypoint");
        way.tag = "DroneWay";
        way.transform.position = cam.transform.position;
        way.transform.rotation = cam.transform.rotation;
        way.transform.SetParent(parent.transform);
        way.AddComponent<DroneWaypoint>();
    }

    [MenuItem("Tools/Create Next Waypoint")]
    public static void CreateNextWaypoint() {
        GameObject parent = Selection.activeGameObject.transform.parent.gameObject;
        
        Transform s = parent.transform.GetChild(parent.transform.childCount - 2);
        Transform e = parent.transform.GetChild(parent.transform.childCount - 1);
        GameObject way = new GameObject("Waypoint");
        way.tag = "DroneWay";
        way.transform.position = e.position + (e.position - s.position);
        way.transform.rotation = e.rotation * (e.rotation * Quaternion.Inverse(s.rotation));
        way.transform.SetParent(parent.transform);
        way.AddComponent<DroneWaypoint>();
    }

    [MenuItem("Tools/Create Next Waypoint")]
    public static bool CanNextWaypoint() {
        return Selection.activeGameObject != null && Selection.activeGameObject.transform.parent != null && Selection.activeGameObject.transform.parent.childCount >= 2;
    }


    [MenuItem("Tools/Goto Drone Waypoint")]
    public static void GotoWaypoint() {
        GameObject o = Selection.activeGameObject;
        SceneView view = SceneView.lastActiveSceneView;

        view.AlignViewToObject(o.transform);
    }

    [MenuItem("Tools/Goto Drone Waypoint", true)]
    public static bool CanGotoWaypoint() {
        return Selection.activeGameObject != null;
    }

    /*
    [MenuItem("Tools/Create Drone Waypoint", true)]
    public static bool CheckCreateWaypoint() {
        Debug.Log(SceneView.currentDrawingSceneView);
        return SceneView.currentDrawingSceneView.camera != null;
    }*/
}
