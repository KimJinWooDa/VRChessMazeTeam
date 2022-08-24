using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseToPlayer : MonoBehaviour
{
    [SerializeField] float speed = 2f;
    [SerializeField] float waitTime = 4f;
    [SerializeField] Transform target;
    [SerializeField] float dashLength = 7f;
    [SerializeField] ParticleSystem ps;
    Renderer renderer;
    Vector3 startPos;
    private void Start()
    {
        renderer = GetComponent<Renderer>();
        ps = GetComponentInChildren<ParticleSystem>();
        startPos = transform.position;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        StartCoroutine(WaitForMoveMent());
    }

    IEnumerator WaitForMoveMent()
    {
        Vector3 dir = (target.position - transform.position).normalized;
        transform.forward = dir;
        Vector3 originPos = transform.position;
        Vector3 targetPos = transform.position + dir * dashLength;

        float t = 0f;
        while (t < waitTime)
        {
            float count = renderer.material.GetFloat("_FinalPower");
            t += Time.deltaTime;
            if (t >= 2)
            {
                ps.gameObject.SetActive(false);
  
                renderer.material.SetFloat("_FinalPower", 10);
            }
            else
            {
                renderer.material.SetFloat("_FinalPower",  (int)count + 1);
                ps.gameObject.SetActive(true);
            }
            transform.rotation = Quaternion.LookRotation(targetPos - transform.position);
            transform.position = Vector3.Lerp(originPos, targetPos, (t / waitTime));
            yield return null;
        }
        transform.position = targetPos;
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(WaitForMoveMent());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<ReStart>().ReStartGame();
            ReStart();
        }
    }

    void ReStart()
    {
        this.transform.rotation = Quaternion.identity;
        this.transform.position = startPos;
        StopCoroutine(WaitForMoveMent());
        StartCoroutine(WaitForMoveMent());
    }
}
