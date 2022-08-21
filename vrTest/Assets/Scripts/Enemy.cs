using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Enemy : MonoBehaviour
{
    public enum State { 이동, 공격 };
    public State state;

    public float hp = 1f;
    public float speed = 2f;

    Image hpImage;
    Rigidbody rb;

    public Transform targetCharacter;
    public bool isStart;
    Vector3 originPosition;

    public bool isTest;
    private void Awake()
    {
        originPosition = this.transform.position;
        hpImage = GetComponentInChildren<Image>();
        rb = GetComponent<Rigidbody>();
    }
    public void StartAttack(bool isOn)
    {
        isStart = isOn;
    }
    public void GetDamage(float damage)
    {
        hp -= damage * 0.01f;
        Vector3 dir = (targetCharacter.position - this.transform.position).normalized;
        rb.AddForce(dir * 2f, ForceMode.Impulse);
    }
    public void Update()
    {
        hpImage.fillAmount = hp;
       
        if (isStart || isTest)
        {
            transform.position = Vector3.MoveTowards(this.transform.position, targetCharacter.position, Time.deltaTime * speed);
            if (canJump)
            {
                canJump = false;
                rb.AddForce(Vector3.up * 3f, ForceMode.Impulse);

            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(this.transform.position, originPosition, Time.deltaTime * speed);
            transform.rotation = Quaternion.Euler(90f, 0, 90f);
        }
    }
    bool canJump = true;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("GROUND"))
        {
            canJump = true;
        }
    }
}
