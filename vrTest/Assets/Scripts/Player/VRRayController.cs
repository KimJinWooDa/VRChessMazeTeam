using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VRRayController : MonoBehaviour
{
    public const int MAGNET_LAYER = 6;
    public const float PULL_STR = 23f, STALL_TIME = 3f, YEET_STR = 600f, HOLD_LINGER = 0.5f, MAGNET_RANGE = 40f;

    [Header("Preset Values")]
    public PlayerControl pcon;
    public GameObject triggerEffectHolder;

    [Header("Debug")]
    public Magnet magnet;//not null if hovering is true, and vice versa
    public bool hovering, connected, lastShoot;
    public ChainCore chain;//not null if lastChain is true, and vice versa


    [Header("Tests/Overrides")]
    [HideInInspector] public Quaternion overrideRotation;
    [HideInInspector] public bool testGrip, testTrigger, overrideTarget = false;

    public enum ControllerType { Left = 0, Right = 1 };
    public ControllerType CT;
    RaycastHit hit;

    private float holdf = 0;
    private bool holdb = false;

    [SerializeField] RotateCharacter rc;
    public bool canTrigger;

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
        this.magnet = magnet;
        magnet.GetComponent<ObjectIsHovering>().IsHovering(true);
        rc.targetMagnet = this.magnet.transform;
        //this.magnet.Hover(CT);
    }

    public void VRExitHovering()
    {
        //hovering = false;
        //magnet.Unhover(CT);
        //this.magnet = null;
        magnet.GetComponent<ObjectIsHovering>().IsHovering(false);
    }
    public void VRExitTrigger()
    {
        //canTrigger = false;
    }
    public void VRTrigger()
    {
        if ((OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))) {
            canTrigger = true;
        }
    }
    public bool isTest;
    void Update()
    {
        if (overrideTarget)
        {
            transform.rotation = overrideRotation;
        }

        if (lastShoot)
        {
            if (!OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger) && !isTest)
            {
                rc.isTriggerState = false;
                rc.StopUp();
                this.magnet = null;
                RemoveChain();
                canTrigger = false;
            }
            else if (!connected)
            {
                if (!chain.expanding)
                {
                    rc.Set(this.magnet.transform, true);
                    GetComponent<Rigidbody>().isKinematic = true;
                    connected = true;
                }
            }
            
        }
        else
        {
            if (magnet!=null && (canTrigger || isTest))
            {
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
            }
        }

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
        Vector3 pos = transform.position;
        float len = Vector3.Distance(pos, target.transform.position);
        if (len > chain.MaxLength())
        {
            pcon.rigid.MovePosition(pcon.rigid.position + (target.transform.position - pos).normalized * (len - chain.MaxLength()));
            if (Vector3.Distance(pos, target.transform.position) > chain.MaxLength() + 0.3f)
            {
                //break the chain, the pull is obstructed and there is no hope whatsoever
                //RemoveChain();
                return;
            }
            len = chain.MaxLength();
        }
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
    { 
        if (lr == ControllerType.Left) return OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger);
        else return OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger);
    }

    public bool IsTrigger(ControllerType lr)
    { 
        if (testTrigger) return true;
        if (lr == ControllerType.Left) return OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger);
        else return OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger);
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