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
        yield return new WaitForSeconds(waitTime);
        Vector3 dir = (target.position - transform.position).normalized;
        transform.forward = dir;
        Vector3 originPos = transform.position;
        Vector3 targetPos = transform.position + dir * dashLength;
        float t = 0f;
        float y, z;
        Color redColor = new Color();
        redColor = new Vector4(1f, 1f, 1f, 1f);
        renderer.material.SetColor("_BaseColor", redColor);
        while (t < waitTime)
        {
            t += Time.deltaTime;
            if (t >= 2)
            {
                ps.gameObject.SetActive(false);
                redColor = new Vector4(1, 0, 0, 1f);
                renderer.material.SetColor("_BaseColor", redColor);
            }
            else
            {
                y = (1 / Time.deltaTime);
                z = (1 / Time.deltaTime);
                
                redColor = new Vector4(1, 1 - y, 1 - z, 1f);
                renderer.material.SetColor("_BaseColor", redColor);
                ps.gameObject.SetActive(true);
            }
            transform.rotation = Quaternion.LookRotation(targetPos - transform.position);
            transform.position = Vector3.Lerp(originPos, targetPos, (t / waitTime));
            yield return null;
        }
        transform.position = targetPos;
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
