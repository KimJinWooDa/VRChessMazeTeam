using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainUtilsTest : MonoBehaviour {
    public GameObject target;
    public bool shoot = false;
    private bool lastShoot = false;

    private ChainCore chain;

    void Start() {
        ChainUtils.Line(transform.position, transform.rotation, 40);
    }

    private void Update() {
        if (chain != null && chain.expanding) shoot = true;

        if (shoot) {
            if (!lastShoot) {
                if (chain != null) StartCoroutine(RemoveChainI(chain));
                chain = ChainUtils.LineTarget(transform.position + Vector3.forward, target);
            }
        }
        else {
            if (chain != null) StartCoroutine(RemoveChainI(chain));
            chain = null;
        }
        lastShoot = shoot;
    }

    IEnumerator RemoveChainI(ChainCore chain) {
        while (chain != null && chain.gameObject != null) {
            chain.offset += Time.deltaTime * 20f;
            chain.transform.position += (chain.target.transform.position - transform.position).normalized * Time.deltaTime * 20f * ChainUtils.SCALE * ChainUtils.LENGTH;
            yield return null;
        }
    }
}
