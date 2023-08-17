using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCheck : MonoBehaviour
{
    public float attackPower;
    public GameObject hitEnemyEffect, hitEffect;

    private void OnTriggerEnter(Collider other)
    {
        transform.SetParent(other.gameObject.transform);

        if (other.gameObject.CompareTag("Enemy"))
        {
            GameObject HitEffect = Instantiate(hitEnemyEffect, transform.position, Quaternion.identity) as GameObject;
            Destroy(HitEffect, 2);
            Enemy enemy = other.GetComponent<Enemy>();
            enemy.ComputeDamage(attackPower);
        }

    }
}
