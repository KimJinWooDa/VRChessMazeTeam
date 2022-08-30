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


    private void Update()
    {
        rayPos = this.transform.position;
        Rotation = this.transform.rotation;
        Forward = Rotation * Vector3.forward;

        if (Physics.Raycast(rayPos, Forward, out hit, rayDistance, 1 << 6))
        {
            canTrigger = true;
            magnet = hit.transform.GetComponent<Magnet>();
            magnet.GetComponentInChildren<ObjectIsHovering>().isHovering = true;
        }
        else
        {
           if(magnet!=null) magnet.GetComponentInChildren<ObjectIsHovering>().isHovering = false;
        }

        if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger) && magnet != null)
        {
            RayState();
        }
        else if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
        {
            Detach();
        }

        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) && magnet != null)
        {
            RayState();
        }
        else if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
        {
            Detach();
        }
    }
    private void FixedUpdate()
    {
        if (!OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) && (!OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger)) && !canTrigger)
        {
            CompletelyDetach();
        }
    }
    public void RayState()
    {
        if (lastShoot)
        {
            if (!connected)
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
            if (magnet != null && canTrigger)
            {
                lastShoot = true;
                PlayerNoAction();
                
                chain = CT == ControllerType.Right ? ChainUtils.LineTarget(rightChainTransform.position, magnet.gameObject) : ChainUtils.LineTarget(leftChainTransform.position, magnet.gameObject);
            }
        }

        if (lastShoot && chain != null)
        {
            chain.transform.position = CT == ControllerType.Left ? leftChainTransform.position: rightChainTransform.position;

            if (connected)
            {
                myCharacter.GetComponent<SimpleCapsuleWithStickMovement>().enabled = false;
                UpdateChainLength(magnet);
                PullTowards(magnet);
                StartCoroutine(DragScaleUp());
            }

        }
    }
    IEnumerator DragScaleUp()
    {
        onceScaleUp = true;
        yield return new WaitForSeconds(0.15f);
        pcon.rigid.drag = 0.8f;
        pcon.rigid.angularDrag = 0.9f;
    }
    public void PlayerNoAction()
    {
        pcon.isFlying = true;
        pcon.canJump = false;
        connected = false;
    }
    public void Detach()
    {
        RemoveChain();

        magnet = null;
        canTrigger = false;
    }

    public void CompletelyDetach()
    {
        pcon.isFlying = false;
        pcon.rigid.drag = 0f;
        pcon.rigid.angularDrag = 0.05f;

        myCharacter.GetComponent<SimpleCapsuleWithStickMovement>().enabled = true;

        this.canTrigger = false;
        if (otherController.canTrigger) otherController.canTrigger = false;

        RemoveChain();
        magnet = null;
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
        pos = CT == ControllerType.Left ? leftChainTransform.position : rightChainTransform.position;
        if (target != null)
        {
            float len;
            len = Vector3.Distance(pos, target.transform.position);
            //pcon.rigid.MovePosition(pcon.rigid.position + (target.transform.position - pos).normalized * (len - chain.MaxLength()));
            if (len > chain.MaxLength())
            {
                if (Vector3.Distance(pos, target.transform.position) > chain.MaxLength() + 0.3f)
                {
                    return;
                }
                len = chain.MaxLength();
            }
            chain.SetLength(len);
        }
    }

    [SerializeField] float pullSpeed = 2f;
  
    private void PullTowards(Magnet target)
    {
        canTrigger = false;
        pos = CT == ControllerType.Left ? leftChainTransform.position : rightChainTransform.position;
        if (target != null)
        {
            float len2;
            len2 = Vector3.Distance(target.transform.position, pos);

            if (len2 < target.openRadius)
            {
                myCharacter.transform.position = Vector3.Lerp(myCharacter.transform.position, target.transform.position, 0.07f);
                pcon.rigid.isKinematic = true;
                pcon.rigid.isKinematic = false;
                target.CheckOpen();
            }
            else
            {
                pcon.rigid.AddExplosionForce(forwardPower * 60f * Time.deltaTime * pullSpeed * Mathf.Clamp01(len2/ (target.radius * 5f)) * -1f, target.transform.position, len2 * 1f);

            }
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
