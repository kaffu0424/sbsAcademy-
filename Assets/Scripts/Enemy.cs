using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent pathFinder;
    Transform target;
    Rigidbody myRigidBody;

    public Animator animator;
    public float refreshRate = .25f;
    public float hp;
    public float attackPower;
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
        animator.SetTrigger("Damage");
        hp -= dmg;
        //print(pathFinder.speed);       

        if(hp<=0)
        {
            //»ç¸Á µ¿ÀÛ
            animator.SetBool("Die", true);
            pathFinder.speed = 0;
            //»ç¸Á effect
            Destroy(this.gameObject,3);
        }
        else
        {
            StartCoroutine(NockBack(pathFinder.speed));
        }
        
    }

    IEnumerator NockBack(float originalSpeed)
    {
        pathFinder.speed = -originalSpeed;
        yield return new WaitForSeconds(0.3f);
        pathFinder.speed = originalSpeed;
    }

}
