using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VRRayController : MonoBehaviour
{
    public const int MAGNET_LAYER = 6;
    public const float PULL_STR = 100f, STALL_TIME = 3f, YEET_STR = 600f, HOLD_LINGER = 0.5f, MAGNET_RANGE = 40f;

    [Header("Preset Values")]
    public PlayerControl pcon;
    public GameObject triggerEffectHolder;

    [Header("Debug")]
    public Magnet magnet;//not null if hovering is true, and vice versa
    public bool hovering, connected, lastShoot;
    public ChainCore chain;//not null if lastChain is true, and vice versa


    [Header("Tests/Overrides")]
    public Quaternion overrideRotation;
    public bool testGrip, testTrigger, overrideTarget = false;

    public enum ControllerType { Left = 0, Right = 1 };
    public ControllerType CT;
    RaycastHit hit;

    private float holdf = 0;
    private bool holdb = false;

    void Start()
    {
        holdb = false;
        holdf = 0;
        triggerEffectHolder.SetActive(false);
    }
    public void PullObject(GameObject ob)
    {

        ob.GetComponent<PullObject>().canPull = true;

    }
    public void VRHovering(Magnet magnet)
    {
        //grip
        if (lastShoot || Physics.Raycast(transform.position, transform.forward, out hit, MAGNET_RANGE, 1 << MAGNET_LAYER) && IsValid(hit))
        {
            if (!hovering)
            {
                hovering = true;
                this.magnet = magnet;
                this.magnet.Hover(CT);
            }
        }
    }

    public void VRExitHovering()
    {
        if (hovering)
        {
            hovering = false;
            magnet.Unhover(CT);
            this.magnet = null;
        }
    }
    public void VRExitTrigger()
    {

    }
    public void VRTrigger()
    {
        if (lastShoot)
        {
            if (!Trigger(CT))
            {
                //detach
                RemoveChain();
            }
            else if (!connected)
            {
                //chain is extending
                if (!chain.expanding)
                {
                    //chain done expanding
                    connected = true;
                }
            }
        }
        else
        {
            if (hovering && Trigger(CT))
            {
                //shoot
                lastShoot = true;
                connected = false;
                chain = ChainUtils.LineTarget(transform.position, magnet.gameObject);
            }
        }

        if (lastShoot && chain != null)
        {
            chain.transform.position = transform.position;
            if (connected)
            {
                UpdateChainLength(magnet);
                PullTowards(magnet);
            }
        }
    }
    void Update()
    {
        //face towards override target (for testing)
        if (overrideTarget)
        {
            transform.rotation = overrideRotation;
        }

        #region 그립
        //grip
        //if (lastShoot || (Grip(CT) && Physics.Raycast(transform.position, transform.forward, out hit, MAGNET_RANGE, 1 << MAGNET_LAYER) && IsValid(hit)))
        //{
        //    if (!hovering)
        //    {
        //        hovering = true;
        //        magnet.Hover(CT);
        //    }
        //}
        //else
        //{
        //    if (hovering)
        //    {
        //        hovering = false;
        //        magnet.Unhover(CT);
        //        magnet = null;
        //    }
        //}
        #endregion

        #region 트리거
        //trigger
        if (lastShoot)
        {
            if (!Trigger(CT))
            {
                //detach
                RemoveChain();
            }
            else if (!connected)
            {
                //chain is extending
                if (!chain.expanding)
                {
                    //chain done expanding
                    connected = true;
                }
            }
        }
        else
        {
            if (hovering && Trigger(CT))
            {
                //shoot
                lastShoot = true;
                connected = false;
                chain = ChainUtils.LineTarget(transform.position, magnet.gameObject);
            }
        }

        if (lastShoot && chain != null)
        {
            chain.transform.position = transform.position;
            if (connected)
            {
                UpdateChainLength(magnet);
                PullTowards(magnet);
            }
        }
        #endregion

        #region HoldingEffect
        //holding effects
        holdf = Mathf.Clamp01(holdf + Time.deltaTime / HOLD_LINGER * (hovering ? 1 : -1));
        if (holdf > 0)
        {
            if (!holdb)
            {
                holdb = true;
                triggerEffectHolder.SetActive(true);
            }
            triggerEffectHolder.transform.localScale = Vector3.one * holdf;
        }
        else
        {
            if (holdb)
            {
                holdb = false;
                triggerEffectHolder.SetActive(false);
            }
        }
        #endregion
    }

    public void RemoveChain()
    {
        if (!lastShoot) return;
        lastShoot = false;
        connected = false;
        if (chain.expanding) StartCoroutine(RemoveChainPreI(chain));
        else StartCoroutine(RemoveChainI(chain));
        chain = null;
    }

    private void UpdateChainLength(Magnet target)
    {
        //rotate core: try to go to pos
        Vector3 pos = transform.position;
        float len = Vector3.Distance(pos, target.transform.position);
        if (len > chain.MaxLength())
        {
            //try to move rigidbody closer
            pcon.rigid.MovePosition(pcon.rigid.position + (target.transform.position - pos).normalized * (len - chain.MaxLength()));
            if (Vector3.Distance(pos, target.transform.position) > chain.MaxLength() + 0.3f)
            {
                //break the chain, the pull is obstructed and there is no hope whatsoever
                RemoveChain();
                return;
            }
            //wrong color
            if (!target.hasColor) pcon.lastColorExists = false;
            if (pcon.lastColorExists && target.white == pcon.lastWhite)
            {
                WrongColor(target);
                StartCoroutine(RemoveChainI(chain));
                chain = null;
                return;
            }
            if (target.hasColor) pcon.lastWhite = target.white;

            len = chain.MaxLength();
        }
        //chainge offset based on len
        chain.SetLength(len);
    }

    private void PullTowards(Magnet target)
    {
        float len = Vector3.Distance(target.transform.position, transform.position);
        //pull with force
        if (len > target.radius)
        {
            pcon.rigid.AddExplosionForce(PULL_STR * /*Mathf.Clamp01((len - target.radius) / (target.radius * 5f)) * */ -1, target.transform.position, len * 2f);
        }
        else
        {
            pcon.rigid.AddExplosionForce(PULL_STR * Mathf.Clamp01(1f - len / (target.radius)), target.transform.position, len * 2f);
        }
        //open cage
        //if (len < target.radius * 2f) target.CheckOpen();
    }

    private void WrongColor(Magnet target)
    {
        pcon.WrongColor();
        pcon.rigid.AddExplosionForce(YEET_STR, target.transform.position, Vector3.Distance(target.transform.position, transform.position) * 2f);
    }

    public static ControllerType Other(ControllerType type)
    {
        if (type == ControllerType.Left) return ControllerType.Right;
        return ControllerType.Left;
    }

    private bool IsValid(RaycastHit hit)
    {
        if (hit.transform.gameObject.TryGetComponent(out magnet) && !magnet.Hovered(Other(CT))) return true;
        magnet = null;
        return false;
    }

    public bool Grip(ControllerType lr)
    { //Grip: activate chain yeet mode
        if (testGrip) return true;
        if (lr == ControllerType.Left) return OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger);
        else return OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger);
    }

    public bool Trigger(ControllerType lr)
    { //Trigger: yeet chain
        if (testTrigger) return true;
        if (lr == ControllerType.Left) return OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger);
        else return OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger);
    }

    IEnumerator RemoveChainI(ChainCore chain)
    {
        while (chain != null && chain.gameObject != null)
        {
            chain.offset += Time.deltaTime * 20f;
            chain.transform.position += (chain.target.transform.position - transform.position).normalized * Time.deltaTime * 20f * ChainUtils.SCALE * ChainUtils.LENGTH;
            yield return null;
        }
    }

    IEnumerator RemoveChainPreI(ChainCore chain)
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