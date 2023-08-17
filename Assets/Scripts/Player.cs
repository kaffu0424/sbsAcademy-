using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    float moveSpeed,moveCount,hp;
    public float limitMove,hpMax;
    public bool enableTurn,enableShoot;
    public int selectedWeaponNum=0;

    public GameObject[] weapons;
    public GameObject swordAttackEffect, spearAttackEffect;
    public GameObject arrow;
    public Transform bowFirePoint,swordAttackPos, spearAttackPos;
    public Rigidbody myRigidbody;

    Camera viewCamera;

    public Animator animator;

    private void Start()
    {
        viewCamera = Camera.main;
        enableTurn = true;
        enableShoot = true;
        moveCount = 1;
        moveSpeed=GameManager.instance.moveSpeed;
        myRigidbody = GetComponent<Rigidbody>();
        WeaponSelect(selectedWeaponNum);
        //swordAttackEffect.SetActive(false);
        //spearAttackEffect.SetActive(false);
        hp = hpMax;
    }

    private void Update()
    {
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        if(groundPlane.Raycast(ray,out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            Vector3 heightCorrectedPoint = new Vector3(point.x, transform.position.y, point.z);
            transform.LookAt(heightCorrectedPoint);
        }

        if(enableTurn)
        {
            Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            Vector3 moveVelocity = moveInput.normalized * moveSpeed;
            myRigidbody.MovePosition(myRigidbody.position + moveVelocity * Time.deltaTime);
            /*
            float moveAmt = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
            float moveAmt2 = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
            Vector3 moveVector = Vector3.right * moveAmt + Vector3.forward * moveAmt2;
            Debug.Log(moveAmt);
            if (moveAmt < -limitMove||moveAmt > limitMove || moveAmt2 > limitMove || moveAmt2 < -limitMove)
            {
                //enableTurn = false;
            }
            transform.Translate(moveVector);
            */
        }
        
        if(Input.GetMouseButtonDown(0)&&enableShoot)
        {
            if(!EventSystem.current.IsPointerOverGameObject())
            {
                switch (selectedWeaponNum)
                {
                    case 0:
                        GameObject Arrow = Instantiate(arrow, bowFirePoint.position, bowFirePoint.rotation) as GameObject;
                        Destroy(Arrow.gameObject, 10);
                        break;
                    case 1:
                        //spearAttackEffect.gameObject.SetActive(true);
                        //StartCoroutine(WaitAndHide(spearAttackEffect));
                        GameObject SpearAttackEffect = Instantiate(spearAttackEffect, spearAttackPos.position, spearAttackPos.rotation) as GameObject;
                        Destroy(SpearAttackEffect, 0.5f);
                        break;
                    case 2:
                        //swordAttackEffect.gameObject.SetActive(true);
                        //StartCoroutine(WaitAndHide(swordAttackEffect));
                        GameObject SwordAttackEffect = Instantiate(swordAttackEffect, swordAttackPos.position, swordAttackPos.rotation) as GameObject;
                        Destroy(SwordAttackEffect, 0.5f);
                        break;
                }
            }
            
            
        }

    }
    /*
    IEnumerator WaitAndHide(GameObject name)
    {
        yield return new WaitForSeconds(0.5f);
        name.SetActive(false);
    }
    */
    public void WeaponSelect(int num)
    {
        selectedWeaponNum = num;
        for (int i = 0; i < weapons.Length; i++)
        {
            if(i!=num)
            {
                weapons[i].SetActive(false);
            }
            else
            {
                weapons[i].SetActive(true);
            }
        }
    }

    public void ComputeDamage(float dmg)
    {
        //animator.SetTrigger("Damage");
        hp -= dmg;
        
        if (hp <= 0)
        {
            //»ç¸Á µ¿ÀÛ
            animator.SetBool("Die", true);
        }
    }
}
