using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VRRayController : MonoBehaviour
{
    public const int MAGNET_LAYER = 6;
    public const float PULL_STR = 45f, STALL_TIME = 3f, YEET_STR = 600f, HOLD_LINGER = 0.5f, MAGNET_RANGE = 40f;

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

    [SerializeField] Transform rightChainTransform;
    [SerializeField] Transform leftChainTransform;
    public string BishopName;

    [SerializeField] int stageNum;
    [SerializeField] float forwardPower;

 
    void Start()
    {
        holdb = false;
        holdf = 0;
        triggerEffectHolder.SetActive(false);
        this.stageNum = GameManager.instance.stageNum;
        switch (stageNum)
        {
            case 1:
                forwardPower = 35f;
                break;

            case 2:
                forwardPower = PULL_STR;
                break;

            default:
                break;
        }
        //forwardPower = PULL_STR;
    }
    public void PullObject(GameObject ob)
    {

        ob.GetComponent<PullObject>().canPull = true;

    }
    public void VRHovering(Magnet magnet)
    {
        this.magnet = magnet;
        //this.magnet.GetComponent<ObjectIsHovering>().IsHovering(true);
        rc.targetMagnet = this.magnet.transform;
        BishopName = this.magnet.name;
    }

    public void VRExitHovering()
    {
        //this.magnet.GetComponent<ObjectIsHovering>().IsHovering(true);
    }

    public void VRTrigger()
    {
        canTrigger = true;
    }
    public bool isTest;
    void Update()
    {
        if (overrideTarget)
        {
            transform.rotation = overrideRotation;
        }
        if (magnet != null)
        {
            BishopName = magnet.name;
        }
        if (lastShoot)
        {
            if (!OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger) && !OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
            {
                rc.isTriggerState = false;
                rc.StopUp();
                this.magnet = null;
                RemoveChain();
                canTrigger = false;
            }
            else if (!connected)
            {
                if (stageNum == 1)
                {
                    if (!chain.expanding)
                    {
                        pcon.isFlying = true;
                        connected = true;
                    }
                }
                else if (stageNum == 2)
                {
                    if (!chain.expanding && BishopName == "BLACKBISHOP")
                    {
                        rc.Set(this.magnet.transform, true);
                        GetComponent<Rigidbody>().isKinematic = true;
                        connected = true;
                    }
                    if (!chain.expanding && BishopName == "WHITEBISHOP")
                    {
                        pcon.isFlying = true;
                        connected = true;
                    }
                }
            }

        }
        else
        {
            if (magnet != null && canTrigger)
            {
                pcon.GetComponent<Rigidbody>().isKinematic = false;
                pcon.canJump = false;
                lastShoot = true;
                connected = false;
                chain = ChainUtils.LineTarget(leftChainTransform.position, magnet.gameObject);
            }
        }

        if (lastShoot && chain != null)
        {
            if (CT == ControllerType.Left)
            {
                chain.transform.position = leftChainTransform.position;
            }
            else if (CT == ControllerType.Right)
            {
                chain.transform.position = rightChainTransform.position;
            }
            else
            {
                chain.transform.position = rightChainTransform.position;
            }

            if (stageNum == 1)
            {
                if (connected)
                {
                    UpdateChainLengthWhite(magnet);
                    PullTowards(magnet);
                }
            }
            else if (stageNum == 2)
            {
                if (connected && BishopName == "BLACKBISHOP")
                {
                    UpdateChainLength(magnet);
                }
                else if (connected && BishopName == "WHITEBISHOP")
                {
                    UpdateChainLengthWhite(magnet);
                    PullTowards(magnet);
                }
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
        Vector3 pos = rightChainTransform.position;
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
    private void UpdateChainLengthWhite(Magnet target)
    {
        Vector3 pos = rightChainTransform.position;
        float len = Vector3.Distance(pos, target.transform.position);
        if (len > chain.MaxLength())
        {
            pcon.rigid.MovePosition(pcon.rigid.position + (target.transform.position - pos).normalized * (len - chain.MaxLength()));
            if (Vector3.Distance(pos, target.transform.position) > chain.MaxLength() + 0.3f)
            {
                //break the chain, the pull is obstructed and there is no hope whatsoever
                RemoveChain();
                return;
            }
            len = chain.MaxLength();
        }
        chain.SetLength(len);
    }
    private void PullTowards(Magnet target)
    {
        float len = Vector3.Distance(target.transform.position, rightChainTransform.position);
        //pull with force
        if (len > target.radius)
        {
            pcon.rigid.AddExplosionForce(forwardPower * -1f, target.transform.position, len * 1f);
        }
        else
        {
            pcon.rigid.AddExplosionForce(forwardPower * Mathf.Clamp01(1f - len / (target.radius)), target.transform.position, len * 1f);

        }
    }

    private void WrongColor(Magnet target)
    {
        pcon.WrongColor();
        pcon.rigid.AddExplosionForce(YEET_STR, target.transform.position, Vector3.Distance(target.transform.position, rightChainTransform.position) * 2f);
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
            chain.transform.position += (chain.target.transform.position - rightChainTransform.position).normalized * Time.deltaTime * 20f * ChainUtils.SCALE * ChainUtils.LENGTH;
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