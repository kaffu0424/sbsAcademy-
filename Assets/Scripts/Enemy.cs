using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent pathFinder;
    Transform target;
    Rigidbody myRigidBody;

    public float refreshRate = .25f;
    public float hp;

    void Start()
    {
        pathFinder = GetComponent<NavMeshAgent>();
        myRigidBody = GetComponent<Rigidbody>();
        target = GameObject.FindGameObjectWithTag("Player").transform;

        StartCoroutine(UpdatePath());
    }

    IEnumerator UpdatePath()
    {       
        while(target!=null)
        {
            Vector3 targetPosition = new Vector3(target.position.x, 0, target.position.z);
            pathFinder.SetDestination(targetPosition);
            yield return new WaitForSeconds(refreshRate);
        }
    }

    public void ComputeDamage(float dmg)
    {
        hp -= dmg;
        //print(pathFinder.speed);
        StartCoroutine(NockBack(pathFinder.speed));

        if(hp<=0)
        {
            //»ç¸Á µ¿ÀÛ
            //»ç¸Á effect
            Destroy(this.gameObject);
        }
    }

    IEnumerator NockBack(float originalSpeed)
    {
        pathFinder.speed = -originalSpeed;
        yield return new WaitForSeconds(0.3f);
        pathFinder.speed = originalSpeed;
    }

}
