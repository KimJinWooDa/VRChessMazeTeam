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
    public float radius = 0.1f;
    public float openRadius = 1f;

    public bool[] hoveredBy = { false, false };

    public enum ChainPosition { left, right };
    public ChainPosition cp;
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
    public int CheckChainPosition()
    {
        if (cp == ChainPosition.left) return 0;
        else return 1;
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
