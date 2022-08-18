using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ChainUtils;

public class ChainLink : MonoBehaviour {
    private const float TIME = 0.3f;
    private float t = 0;
    private bool anim = true;
    public MeshRenderer mesh;
    public GameObject chainFx;

    public Vector3 pos;
    public int id;
    public ChainCore core;
    public bool end;

    private void Awake() {
        anim = true;
        t = 0;
    }

    void Update() {
        if (anim) {
            t += Time.deltaTime;
            if (t >= TIME) {
                transform.localPosition = pos;
                transform.localRotation = Quaternion.identity;
                transform.localScale = Vector3.one;
                anim = false;
                if (end) {
                    core.expanding = false;
                    if (core.target != null) core.Link();
                    Instantiate(chainFx, transform.position, Quaternion.identity);
                }
            }
            else {
                transform.localPosition = pos * (t / TIME) + transform.localPosition * (1 - t / TIME);
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.identity, t / TIME);
                transform.localScale = Vector3.one * t / TIME;
            }
        }
        else {
            //transform.localPosition = pos + Vector3.down / SCALE * LENGTH * core.offset;
            transform.localScale = Vector3.one * Mathf.Clamp01((core.LENGTH - id) + 1 - core.offset);
            mesh.enabled = core.LENGTH - id > core.offset - 1;
        }
    }
}
