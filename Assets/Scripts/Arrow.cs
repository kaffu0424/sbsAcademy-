using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 10;
    public float attackPower;
    public GameObject hitEffect, hitEnemyEffect;

    private void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        speed = 0;
        transform.SetParent(other.gameObject.transform);
        
        if(other.gameObject.CompareTag("Enemy"))
        {
            GameObject HitEffect = Instantiate(hitEnemyEffect, transform.position, Quaternion.identity) as GameObject;
            Destroy(HitEffect, 2);
            Enemy enemy = other.GetComponent<Enemy>();
            enemy.ComputeDamage(attackPower);            
        }
        else
        {
            GameObject HitEffect = Instantiate(hitEffect, transform.position, Quaternion.identity) as GameObject;
            Destroy(HitEffect, 2);
        }

    }
}
