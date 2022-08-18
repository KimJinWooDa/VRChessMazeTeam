using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBetween : MonoBehaviour {
    public Vector3[] flags;
    public float duration = 3;
    private float t = 0;
    private Vector3 startpos;
    private bool started = false;

    void Start() {
        t = 0;
        startpos = transform.position;
        started = false;
        if(flags.Length == 0) enabled = false;
    }

    void Update() {
        t += Time.deltaTime;
        if (started) {
            int now = Mathf.FloorToInt(t / duration) % flags.Length;
            int next = (now + 1) % flags.Length;
            float f = (t % duration) / duration;
            transform.position = flags[now] * (1 - f) + flags[next] * f;
        }
        else {
            if(t > duration) {
                t = 0;
                transform.position = flags[0];
                started = true;
                if(flags.Length == 1) enabled = false;
            }
            else {
                float f = t / duration;
                transform.position = startpos * (1 - f) + flags[0] * f;
            }
        }
    }
}
