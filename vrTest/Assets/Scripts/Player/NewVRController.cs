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

    public bool isHovering;

    Quaternion Rotation;
    Vector3 Forward;

    private void Update()
    {
        rayPos = this.transform.position;
        Rotation = this.transform.rotation;
        Forward = Rotation * Vector3.forward;

        if (Physics.Raycast(rayPos, Forward, out hit, rayDistance, 1<<6))
        {
            isHovering = true;
            magnet = hit.transform.GetComponent<Magnet>();
            canTrigger = true;
            this.magnet.GetComponentInChildren<ObjectIsHovering>().isHovering = true;
            if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger) && CT == ControllerType.Right)
            {
                RayState();
            }
            else if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger) && CT == ControllerType.Right && otherController != null && otherController.connected)
            {
                Detach();
            }

            if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) && CT == ControllerType.Left)
            {
                RayState();
            }
            else if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger) && CT == ControllerType.Left && otherController != null && otherController.connected)
            {
                Detach();
            }
        }
        else
        {
            if (magnet != null)
            {
                magnet.GetComponentInChildren<ObjectIsHovering>().isHovering = false;
                
                canTrigger = false;
                isHovering = false;
                magnet = null;
            }
        }
    }
    private void FixedUpdate()
    {
        if (!OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) && (!OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger)) && !isHovering)
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
            if (magnet != null && canTrigger) // || isTest)
            {
                PlayerNoAction();
                if(CT == ControllerType.Right)
                {
                    chain = ChainUtils.LineTarget(rightChainTransform.position, magnet.gameObject);
                }
                else
                {
                    chain = ChainUtils.LineTarget(leftChainTransform.position, magnet.gameObject);
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
        yield return new WaitForSeconds(0.23f);
        pcon.rigid.drag = 0.9f;
        pcon.rigid.angularDrag = 0.9f;
    }
    public void PlayerNoAction()
    {
        pcon.canJump = false;
        lastShoot = true;
        connected = false;
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

        pcon.rigid.AddExplosionForce(forwardPower * Time.deltaTime * pullSpeed * 60f * Mathf.Clamp01(len / (target.radius * 5f)) * -1f, target.transform.position, len * 1f);


        if (len < target.openRadius) target.CheckOpen();
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
