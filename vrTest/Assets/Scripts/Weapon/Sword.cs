using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    float power = 20f;

    public override void Awake()
    {
        base.Awake();
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("ENEMY"))
        {
            Transform enemy = collision.collider.transform;

            enemy.GetComponent<Enemy>().GetDamage(power);
        }
    }
}
