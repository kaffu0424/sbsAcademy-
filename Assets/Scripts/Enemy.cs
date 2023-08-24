using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent pathFinder;
    Transform target;
    Rigidbody myRigidBody;
    public GameObject dieEffect,coinEffect;
    public Animator animator;
    public float refreshRate = .25f;
    public float hp, attackPower;
    public int dropExp,minDropCoin,maxDropCoin;
    public bool alive=true;

    Player player;

    void Start()
    {
        pathFinder = GetComponent<NavMeshAgent>();
        myRigidBody = GetComponent<Rigidbody>();
        target = GameObject.FindGameObjectWithTag("Player").transform;

        StartCoroutine(UpdatePath());
    }

    IEnumerator UpdatePath()
    {       
        while(target!=null && hp>0)
        {
            Vector3 targetPosition = new Vector3(target.position.x, 0, target.position.z);
            if(targetPosition!=transform.position)
            {
                animator.SetBool("Walk", true);
                animator.SetBool("Idle", false);
            }
            else
            {
                animator.SetBool("Walk", false);
                animator.SetBool("Idle", true);
            }

            float distance = Vector3.Distance(targetPosition, transform.position);
            if(distance<3)
            {
                animator.SetTrigger("Attack");
                player = target.GetComponent<Player>();
                StartCoroutine(WaitAndAttack(player));
            }

            pathFinder.SetDestination(targetPosition);
            yield return new WaitForSeconds(refreshRate);
        }
    }

    IEnumerator WaitAndAttack(Player player)
    {
        yield return new WaitForSeconds(1.02f);
        player.ComputeDamage(attackPower);
    }

    public void ComputeDamage(float dmg)
    {
        if(alive)
        {
            animator.SetTrigger("Damage");
            hp -= dmg;
            //print(pathFinder.speed);       

            if (hp <= 0)
            {
                alive = false;
                //Debug.Log("¿˚ ªÁ∏¡");
                GameManager.instance.GetExp(dropExp);
                int randomCoin = Random.Range(minDropCoin, maxDropCoin);
                GameManager.instance.UpdateCoin(randomCoin);
                if(randomCoin>0)
                {
                    GameObject CoinEffect = Instantiate(coinEffect, transform.position,Quaternion.identity)as GameObject;
                    Destroy(CoinEffect, 1);
                }
                //ªÁ∏¡ µø¿€
                animator.SetBool("Die", true);
                pathFinder.speed = 0;
                //ªÁ∏¡ effect
                GameObject DieEffect = Instantiate(dieEffect, transform.position, Quaternion.identity) as GameObject;
                Destroy(DieEffect, 1.5f);
                Destroy(myRigidBody);
                Destroy(this.gameObject, 3);
            }
            else
            {
                StartCoroutine(NockBack(pathFinder.speed));
            }
        }
               
    }

    IEnumerator NockBack(float originalSpeed)
    {
        pathFinder.speed = -originalSpeed;
        yield return new WaitForSeconds(0.3f);
        pathFinder.speed = originalSpeed;
    }

}
