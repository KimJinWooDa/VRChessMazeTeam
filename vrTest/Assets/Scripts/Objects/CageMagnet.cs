using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageMagnet : Magnet {
    public GameObject pawn;
    public bool attachComponents;

    public override void Awake() {
        if (attachComponents) {
            BoxCollider col = gameObject.AddComponent<BoxCollider>();
            col.size = new Vector3(40.17f, 60.71f, 36.53f);
            col.isTrigger = true;
            /*
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = true;*/
            base.Awake();
        }
        else {
            base.Awake();
        }
    }

    public override void Open() {
        //give the player the pawn trapped inside
        //폰을 플레이어에게 움직이도록 애니메이션.
    }
}
