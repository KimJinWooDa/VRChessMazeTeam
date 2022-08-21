using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowChain : MonoBehaviour
{
    ChainCore chain;
    [SerializeField] Magnet magnet  = null;

    bool isOnce;
    public void OnTargeting(Magnet magnet)
    {
        this.magnet = magnet;
        isOnce = false;
    }

    public void OffTargeting()
    {
        magnet = null;
        isOnce = true;
    }
    private void Update()
    {
        if (magnet != null && !isOnce)
        {
            isOnce = true;
            chain = ChainUtils.LineTarget(transform.position, magnet.gameObject);
            if (chain != null)
            {
                chain.transform.position = transform.position;
                UpdateChainLength(magnet);
            }
        }

    }
    private void UpdateChainLength(Magnet target)
    {
        Vector3 pos = transform.position;
        float len = Vector3.Distance(pos, target.transform.position);
        if (len > chain.MaxLength())
        {
            if (Vector3.Distance(pos, target.transform.position) > chain.MaxLength() + 0.3f)
            {
                RemoveChain();
                return;
            }
            len = chain.MaxLength();
        }
        chain.SetLength(len);
    }

    public void RemoveChain()
    {
        if (chain.expanding) StartCoroutine(RemoveChainPreI(chain));
        else StartCoroutine(RemoveChainI(chain));
        chain = null;
    }

    public IEnumerator RemoveChainI(ChainCore chain)
    {
        while (chain != null && chain.gameObject != null)
        {
            chain.offset += Time.deltaTime * 20f;
            chain.transform.position += (chain.target.transform.position - transform.position).normalized * Time.deltaTime * 20f * ChainUtils.SCALE * ChainUtils.LENGTH;
            yield return null;
        }
    }

    public IEnumerator RemoveChainPreI(ChainCore chain)
    {
        chain.enabled = false;
        float t = 0;
        while (t < 0.5f)
        {
            t += Time.deltaTime * 20f;
            chain.transform.localScale = Vector3.one * ChainUtils.SCALE * (t / 0.5f);
            yield return null;
        }
        Destroy(chain.gameObject);
    }
}
