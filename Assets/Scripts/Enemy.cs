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
    public float hp, attackPower,pathFindingSpeed;
    public int dropExp,minDropCoin,maxDropCoin;
    public int lv = 1;
    public bool alive=true;

    Player player;

    void Start()
    {
        pathFinder = GetComponent<NavMeshAgent>();
        myRigidBody = GetComponent<Rigidbody>();
        target = GameObject.FindGameObjectWithTag("Player").transform;

        //레벨에 따라 자동으로 변화하는 수치 처리
        hp *= ((hp / 100 * lv) + 1);
        attackPower *= ((attackPower / 100 * lv) + 1);
        dropExp *= ((dropExp / 100 * lv) + 1);
        minDropCoin *= ((minDropCoin / 100 * lv) + 1);
        maxDropCoin *= ((maxDropCoin / 100 * lv) + 1);


        StartCoroutine(UpdatePath());
        //print(pathFinder.speed);
        pathFinder.speed = pathFindingSpeed;
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
                pathFinder.speed = 0;
                animator.SetTrigger("Attack");
                player = target.GetComponent<Player>();
                //StartCoroutine(WaitAndAttack(player));
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

    public void DoAttack()
    {
        if((target != null && hp > 0))
        {
            Vector3 targetPosition = new Vector3(target.position.x, 0, target.position.z);
            float distance = Vector3.Distance(targetPosition, transform.position);
            if(distance<3)
            {

                player.ComputeDamage(attackPower);
            }
            //pathFinder.enabled = true;
            pathFinder.speed = 3.5f;
        }
        
    }

    public void ComputeDamage(float dmg)
    {
        if(alive)
        {
            pathFinder.speed = 0;
            animator.SetTrigger("Damage");
            hp -= dmg;
            //print(pathFinder.speed);       

            if (hp <= 0)
            {
                alive = false;
                //Debug.Log("적 사망");
                GameManager.instance.GetExp(dropExp);
                int randomCoin = Random.Range(minDropCoin, maxDropCoin);
                GameManager.instance.UpdateCoin(randomCoin);
                if(randomCoin>0)
                {
                    GameObject CoinEffect = Instantiate(coinEffect, transform.position,Quaternion.identity)as GameObject;
                    Destroy(CoinEffect, 1);
                }
                //사망 동작
                animator.SetBool("Die", true);
                pathFinder.speed = 0;
                //사망 effect
                GameObject DieEffect = Instantiate(dieEffect, transform.position, Quaternion.identity) as GameObject;
                Destroy(DieEffect, 1.5f);
                Destroy(myRigidBody);
                Destroy(this.gameObject, 3);
            }
            else
            {
                //StartCoroutine(NockBack(pathFinder.speed));
            }
            StartCoroutine(WaitAndResume());
        }
               
    }

    IEnumerator WaitAndResume()
    {
        yield return new WaitForSeconds(0.4f);
        pathFinder.speed = 3.5f;
    }

    IEnumerator NockBack(float originalSpeed)
    {
        pathFinder.speed = -originalSpeed;
        yield return new WaitForSeconds(0.3f);
        pathFinder.speed = originalSpeed;
    }

}
