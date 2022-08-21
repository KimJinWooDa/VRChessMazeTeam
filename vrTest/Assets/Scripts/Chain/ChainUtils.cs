using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainUtils : MonoBehaviour {
    public const float LENGTH = 0.06f;
    public const float SCALE = 2f;
    public const float DELAY = 0.01f;
    public static ChainUtils main;
    public static GameObject chainPrefab;
    public static WaitForSeconds delayer;
    public GameObject chainPrefabHook;

    public static ChainCore Line(Vector3 start, Quaternion direction, int n) {
        GameObject holder = new GameObject("Chains");
        ChainCore core = holder.AddComponent<ChainCore>();
        core.LENGTH = n;
        core.expanding = true;
        holder.transform.position = start;
        holder.transform.rotation = direction * Quaternion.AngleAxis(-90, Vector3.left);
        holder.transform.localScale = new Vector3(SCALE, SCALE, SCALE);
        main.StartCoroutine(LineI(core, n));
        return core;
    }

    public static GameObject GetChain(Vector3 pos) {
        return Instantiate(chainPrefab, pos, Random.rotation);
    }

    public static ChainCore LineWorld(Vector3 start, Quaternion direction, float length) {
        return Line(start, direction, Mathf.CeilToInt(length / (SCALE * LENGTH)));
    }

    public static ChainCore LineTarget(Vector3 start, GameObject target) {
        GameObject holder = new GameObject("Chains");
        ChainCore core = holder.AddComponent<ChainCore>();

        core.LENGTH = 400; //maximum value as the target keeps moving
        core.expanding = true;
        core.target = target;
        holder.transform.position = start;
        holder.transform.rotation = Quaternion.LookRotation(target.transform.position - start, Vector3.up) * Quaternion.AngleAxis(-90, Vector3.left);
        holder.transform.localScale = new Vector3(SCALE, SCALE, SCALE);

        main.StartCoroutine(LineTargetI(core, target));
        return core;
    }

    private void Awake() {
        main = this;
        chainPrefab = chainPrefabHook;
        delayer = new WaitForSeconds(DELAY);
    }

    static IEnumerator LineI(ChainCore holder, int n) {
        for (int i = 0; i < n; i++) {
            ChainLink chain = GetChain(Vector3.up * LENGTH * i + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f))).GetComponent<ChainLink>();//todo pooling
            chain.pos = Vector3.up * LENGTH * i;
            chain.transform.SetParent(holder.transform, false);
            chain.id = i;
            chain.core = holder;
            if (i == n - 1) {
                chain.end = true;
            }
            yield return delayer;
        }
    }

    static IEnumerator LineTargetI(ChainCore holder, GameObject target) {
        int i = 0;
        ChainLink chain = null;
        while (i * LENGTH * SCALE < Vector3.Distance(holder.transform.position, target.transform.position)) {
            if(i > 0 && i % 4 == 3) yield return delayer;
            chain = GetChain(Vector3.up * LENGTH * i + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f))).GetComponent<ChainLink>();//todo pooling
            chain.pos = Vector3.up * LENGTH * i;
            chain.transform.SetParent(holder.transform, false);
            chain.id = i;
            chain.core = holder;
            i++;
        }
        if (chain != null) chain.end = true;
        holder.LENGTH = i;
        //holder.offset = i - Vector3.Distance(holder.transform.position, target.transform.position) / (SCALE * LENGTH);
        //holder.Link();
    }
}
