using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseToPlayer : MonoBehaviour
{
    [SerializeField] float speed = 2f;
    [SerializeField] float waitTime = 3f, dashTime = 1f;
    [SerializeField] Transform target;
    [SerializeField] float dashLength = 7f;
    [SerializeField] ParticleSystem ps;
    Renderer renderer;
    [SerializeField] Vector3 startPos;
    Rigidbody rb;
    public bool reStart;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();  
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

    bool isEnter;
    IEnumerator WaitForMoveMent()
    {
        yield return new WaitForSeconds(waitTime);
        ps.gameObject.SetActive(true);

        int count = renderer.material.GetInt("_FinalPower");
        count = 1;
        float p = 0;

        Vector3 dir = (target.position - transform.position).normalized;
        Quaternion cq = transform.rotation;
        Quaternion eq = Quaternion.LookRotation(dir, Vector3.up);
        while (p < absorbTime)
        {
            p += Time.deltaTime;
            renderer.material.SetInt("_FinalPower", count + 1);
            transform.rotation = Quaternion.Slerp(cq, eq, Mathf.Clamp01(p / absorbTime * 3f));
            yield return null;
        }
        ps.gameObject.SetActive(false);


        targetPos = transform.position + dir * dashLength;
        originPos = this.transform.position;
        isOnce = true;
        float t = 0;
        while (t < 2f)
        {
            yield return null;
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, targetPos, (t / dashTime) * Time.deltaTime * 2f);
            transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.Euler(-90f, 90f, 90f), (t / dashTime) * Time.deltaTime * 2f);
        }
        renderer.material.SetFloat("_FinalPower", 1f);
        
        StartCoroutine(WaitForMoveMent());
    }
    float timer = 0f;
    bool isOnce;
    Vector3 targetPos;
    Vector3 originPos;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
            other.GetComponent<ReStart>().ReStartGame();

            ReStart();
        }
    }

    void ReStart()
    {
        this.transform.rotation = Quaternion.Euler(-90f, 90f, 90f);
        this.transform.position = startPos; //왜안되지?
        ps.gameObject.SetActive(false);
        Debug.Log("RESTART");
        StartCoroutine(WaitForMoveMent());
    }
}
