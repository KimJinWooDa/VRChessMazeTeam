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

    bool isEnter;
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

        //transform.forward = dir;

        yield return new WaitForSeconds(2f);

        Vector3 dir = (target.position - transform.position).normalized;
        t = 0f;
        targetPos = transform.position + dir * dashLength;
        originPos = this.transform.position;
        isOnce = true;

        while (t < waitTime)
        {

            t += Time.deltaTime;
            transform.position = Vector3.Lerp(this.transform.position, targetPos, (t / waitTime));
            yield return null;
        }

        renderer.material.SetFloat("_FinalPower", 1f);
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(WaitForMoveMent());
    }
    float timer = 0f;
    bool isOnce;
    Vector3 targetPos;
    float t;
    Vector3 originPos;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopCoroutine(WaitForMoveMent());
            other.GetComponent<ReStart>().ReStartGame();

            ReStart();
        }
    }

    void ReStart()
    {
        this.transform.rotation = Quaternion.Euler(90f, 90f, 90f);
        this.transform.position = startPos; //왜안되지?
    }
}
