using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewVRController : MonoBehaviour
{
    public PlayerControl pcon;
    public GameObject triggerEffectHolder;
    public Magnet magnet;
    public ChainCore chain;
    [SerializeField] NewVRController otherController;
    public enum ControllerType { Left = 0, Right = 1 };
    public ControllerType CT;

    [SerializeField] Vector3 rayPos;
    [SerializeField] Transform rightChainTransform;
    [SerializeField] Transform leftChainTransform;
    [SerializeField] Transform myCharacter;

    public bool canTrigger;

    [SerializeField] float rayDistance = 20f;
    [SerializeField] float forwardPower = 23f;

    RaycastHit hit;

    public bool hovering, connected, lastShoot;

    bool onceScaleUp;
    public bool isTest;
    bool plz;

    Quaternion Rotation;
    Vector3 Forward;
    Vector3 pos;
    private float holdf = 0;
    private bool holdb = false;

    private void Start()
    {
        myCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        holdb = false;
        holdf = 0;
        triggerEffectHolder.SetActive(false);
        //this.stageNum = GameManager.instance.stageNum;
        forwardPower = 20f;
    }

    private void Update()
    {
        rayPos = this.transform.position;
        Rotation = this.transform.rotation;
        Forward = Rotation * Vector3.forward;

        if (Physics.Raycast(rayPos, Forward, out hit, rayDistance, 1 << 6))
        {
            if(magnet == null)
            {       
                magnet = hit.transform.GetComponent<Magnet>();
                magnet.GetComponentInChildren<ObjectIsHovering>().isHovering = true;
            }

        }
        else
        {
            if (magnet != null)
            {
                magnet.GetComponentInChildren<ObjectIsHovering>().isHovering = false;
            }
        }

        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) && magnet != null)
        {
            canTrigger = true;
        }

        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) && magnet != null)
        {
            canTrigger = true;
        }

        if (lastShoot)
        {
            if ((!OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger)) && CT == ControllerType.Right && otherController != null && otherController.connected) //  && !isTest
            {
                Detach();
            }
            else if (!OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) && CT == ControllerType.Left && otherController != null && otherController.connected)//!isTest
            {
                Detach();
            }
            else if (!OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) && (!OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))) // && !isTest
            {
                CompletelyDetach();
            }
            else if (!connected)
            {
                if (!chain.expanding)
                {
                    pcon.isFlying = true;
                    connected = true;
                }
            }
        }
        else
        {
            if (magnet != null && (canTrigger)) //
            {
                pcon.canJump = false;
                lastShoot = true;
                connected = false;
                chain = CT == ControllerType.Left ? ChainUtils.LineTarget(leftChainTransform.position, magnet.gameObject) : chain = ChainUtils.LineTarget(rightChainTransform.position, magnet.gameObject);
            }
        }

        if (lastShoot && chain != null)
        {
            chain.transform.position = CT == ControllerType.Right ? rightChainTransform.position : leftChainTransform.position;

            if (connected)
            {
                myCharacter.GetComponent<SimpleCapsuleWithStickMovement>().enabled = false;
                UpdateChainLength(magnet);
                PullTowards(magnet);
                StartCoroutine(DragScaleUp());
            }

        }




        holdf = Mathf.Clamp01(holdf + Time.deltaTime / 0.5f * (hovering ? 1 : -1));
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
        pos = CT == ControllerType.Right ? rightChainTransform.position : leftChainTransform.position;

        float len = Vector3.Distance(target.transform.position, pos);
        //pull with force

        if (len < target.openRadius)
        {
            target.CheckOpen();
        }
        else
        {
            pcon.rigid.AddExplosionForce(forwardPower * Time.deltaTime * pullSpeed * 60f * Mathf.Clamp01(len / (target.radius * 5f)) * -1f, target.transform.position, len * 1f);
        }
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
