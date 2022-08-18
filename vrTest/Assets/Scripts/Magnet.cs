using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

using static VRRayController;

public class Magnet : MonoBehaviour {
    public bool opened;
    public bool hasColor = true;
    public bool white = true;
    public float radius = 1f;

    public bool[] hoveredBy = { false, false };

    public virtual void Awake() {
        SetLayerRecursively(gameObject, MAGNET_LAYER);
    }

    public virtual void Start() {
        opened = false;
    }

    public virtual void CheckOpen() {
        if (!opened) {
            Open();
            opened = true;
        }
    }

    public virtual void Open() {
    }

    public void Hover(ControllerType type) {
        hoveredBy[(int)type] = true;
    }

    public void Unhover(ControllerType type) {
        hoveredBy[((int)type)] = false;
    }

    public bool Hovered(ControllerType type) {
        return hoveredBy[(int)type];
    }

    void SetLayerRecursively(GameObject go, int layer) {
        go.layer = layer;
        Transform t = go.transform;
        for (int i = 0; i < t.childCount; i++)
            SetLayerRecursively(t.GetChild(i).gameObject, layer);
    }
}
