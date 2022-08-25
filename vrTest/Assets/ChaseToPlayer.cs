using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseToPlayer : MonoBehaviour
{
    [SerializeField] float speed = 2f;
    [SerializeField] float waitTime = 3f;
    [SerializeField] Transform target;
    [SerializeField] float dashLength = 7f;
    [SerializeField] ParticleSystem ps;
    Renderer renderer;
    [SerializeField] Vector3 startPos;

    public bool reStart;
    private void Awake()
    {
        startPos = transform.position;
    }
    private void Start()
    {
        renderer = GetComponent<Renderer>();
        ps = GetComponentInChildren<ParticleSystem>();
        
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        StartCoroutine(WaitForMoveMent());
    }
    float absorbTime = 3f;
    float p = 0f;
    IEnumerator WaitForMoveMent()
    {
        p = 0f;
        ps.gameObject.SetActive(true);
        int count = renderer.material.GetInt("_FinalPower");
        count = 1;
        while (p < absorbTime)
        {
            p += Time.deltaTime;
            renderer.material.SetInt("_FinalPower", count + 1);
            yield return null;
        }
        ps.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);

        Vector3 dir = (target.position - transform.position).normalized;
        //transform.forward = dir;
        Vector3 originPos = transform.position;
        Vector3 targetPos = transform.position + dir * dashLength;

        float t = 0f;

        while (t < waitTime)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(originPos, targetPos, (t / waitTime));
            yield return null;
        }
        transform.position = targetPos;
        renderer.material.SetFloat("_FinalPower", 1f);
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
        this.transform.rotation = Quaternion.Euler(90f, 90f, 90f);
        this.transform.position = startPos; //왜안되지?
        StopCoroutine(WaitForMoveMent());
        StartCoroutine(WaitForMoveMent());
    }

    public void OffMovement()
    {
        StopCoroutine(WaitForMoveMent());
    }

    public void OnMoveMent()
    {
        StartCoroutine(WaitForMoveMent());
    }
}
