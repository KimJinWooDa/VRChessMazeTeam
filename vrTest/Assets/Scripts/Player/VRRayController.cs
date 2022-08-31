using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VRRayController : MonoBehaviour
{
    public const int MAGNET_LAYER = 6;
    public const float PULL_STR = 20f, STALL_TIME = 3f, YEET_STR = 600f, HOLD_LINGER = 0.5f, MAGNET_RANGE = 25f, PULL_STR2 = 45f;

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

    [SerializeField] bool isTest;
    Transform myCharacter;

    [SerializeField] VRRayController otherController;
    void Start()
    {
        myCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        holdb = false;
        holdf = 0;
        triggerEffectHolder.SetActive(false);
        this.stageNum = GameManager.instance.stageNum;
        switch (stageNum)
        {
            case 2:
                forwardPower = PULL_STR2;
                break;

            default:
                forwardPower = PULL_STR;
                break;
        }
    }
    public void PullObject(GameObject ob)
    {

        ob.GetComponent<PullObject>().canPull = true;

    }
    public void VRHovering(Magnet magnet)
    {
        this.magnet = magnet;
        //this.magnet.GetComponent<ObjectIsHovering>().IsHovering(true);
        if (rc != null) rc.targetMagnet = this.magnet.transform;
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

    void Update()
    {
        stageNum = GameManager.instance.stageNum;
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
            if ((!OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger)) && CT == ControllerType.Right && otherController != null && otherController.connected && !isTest) // 
            {
                Detach();
            }
            else if (!OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) && CT == ControllerType.Left && otherController != null && otherController.connected && !isTest)
            {
                Detach();
            }
            else if (!OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) && (!OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger)) && !isTest)
            {
                CompletelyDetach();
            }
            else if (!connected)
            {
                if (stageNum == 2)
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
                else
                {
                    if (!chain.expanding)
                    {
                        pcon.isFlying = true;
                        connected = true;
                    }
                }
            }

        }
        else
        {
            if (magnet != null && (canTrigger || isTest)) //
            {
                pcon.canJump = false;
                lastShoot = true;
                connected = false;
                if (CT == ControllerType.Left)
                {
                    chain = ChainUtils.LineTarget(leftChainTransform.position, magnet.gameObject);
                }
                else
                {
                    chain = ChainUtils.LineTarget(rightChainTransform.position, magnet.gameObject);
                }
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

            if (stageNum == 2)
            {
                myCharacter.GetComponent<SimpleCapsuleWithStickMovement>().enabled = false;
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
            else
            {
                if (connected)
                {
                    myCharacter.GetComponent<SimpleCapsuleWithStickMovement>().enabled = false;
                    UpdateChainLength(magnet);
                    PullTowards(magnet);
                    StartCoroutine(DragScaleUp());
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

    IEnumerator DragScaleUp()
    {
        yield return new WaitForSeconds(0.23f);
        pcon.rigid.drag = 0.9f;
        pcon.rigid.angularDrag = 0.9f;
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

    public void Detach()
    {
        magnet = null;
        RemoveChain();
        canTrigger = false;
    }

    public void CompletelyDetach()
    {
        if (stageNum == 2)
        {
            rc.isTriggerState = false;
            rc.StopUp();
        }
        myCharacter.GetComponent<PlayerControl>().isFlying = false;
        pcon.rigid.drag = 0f;
        pcon.rigid.angularDrag = 0.05f;
        myCharacter.GetComponent<SimpleCapsuleWithStickMovement>().enabled = true;

        magnet = null;
        RemoveChain();
        canTrigger = false;
    }

    private void UpdateChainLength(Magnet target)
    {
        Vector3 pos;
        if (CT == ControllerType.Left)
        {
            pos = leftChainTransform.position;
        }
        else
        {
            pos = rightChainTransform.position;
        }

        float len = Vector3.Distance(pos, target.transform.position);
        if (len > chain.MaxLength())
        {
            pcon.rigid.MovePosition(pcon.rigid.position + (target.transform.position - pos).normalized * (len - chain.MaxLength()));
            if (Vector3.Distance(pos, target.transform.position) > chain.MaxLength() + 0.3f)
            {

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
                RemoveChain();
                return;
            }
            len = chain.MaxLength();
        }
        chain.SetLength(len);
    }
    [SerializeField] float pullSpeed = 2f;
    private void PullTowards(Magnet target)
    {
        Vector3 pos;
        if (CT == ControllerType.Left)
        {
            pos = leftChainTransform.position;
        }
        else
        {
            pos = rightChainTransform.position;
        }
        float len = Vector3.Distance(target.transform.position, pos);
        //pull with force
        if (true)
        {
            pcon.rigid.AddExplosionForce(forwardPower * Time.deltaTime * pullSpeed * 60f * Mathf.Clamp01(len / (target.radius * 5f)) * -1f, target.transform.position, len * 1f);
        }

        if (len < target.openRadius)
        {
            target.CheckOpen();
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
    {
        if (testGrip) return true;
        if (lr == ControllerType.Left) return OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger);
        else return OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger);
    }

    public bool Trigger(ControllerType lr)
    {
        if (lr == ControllerType.Left) return OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger);
        else return OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger);
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