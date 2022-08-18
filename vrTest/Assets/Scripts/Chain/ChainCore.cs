using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainCore : MonoBehaviour {
    public int LENGTH;
    [Range(0, 100)] public float offset;
    public GameObject target;
    public bool linked;
    public bool expanding = true;

    private Vector3 lastp;

    void Update() {
        if (target != null) {
            transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position, Vector3.up) * Quaternion.AngleAxis(-90, Vector3.left);
            if (linked) {
                //transform.position += (target.transform.position - lastp);
                //lastp = target.transform.position;
            }
        }
        if(offset > LENGTH) Destroy(gameObject);
    }

    public float RealLength() {
        return (LENGTH - offset) * ChainUtils.SCALE * ChainUtils.LENGTH;
    }

    public float MaxLength() {
        return LENGTH * ChainUtils.SCALE * ChainUtils.LENGTH;
    }

    public void SetLength(float l) {
        offset = Mathf.Clamp(LENGTH - l / (ChainUtils.SCALE * ChainUtils.LENGTH), 0f, LENGTH + 1);
    }

    public void Link() {
        linked = true;
        lastp = target.transform.position;
    }
}
