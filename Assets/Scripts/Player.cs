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
    public GameObject swordAttackEffect, spearAttackEffect, hitEffect,body;
    public GameObject arrow;
    public GameObject dmgImage;
    public Transform bowFirePoint,swordAttackPos, spearAttackPos;
    Rigidbody myRigidbody;
    Camera viewCamera;
    MapGenerator map;

    public Animator animator;

    private void Start()
    {
        map = GameObject.Find("MapGenerator").GetComponent<MapGenerator>();
        viewCamera = Camera.main;
        enableTurn = true;
        enableShoot = true;
        moveCount = 1;
        moveSpeed=GameManager.instance.moveSpeed;
        myRigidbody = GetComponent<Rigidbody>();
        WeaponSelect(selectedWeaponNum);
        //swordAttackEffect.SetActive(false);
        //spearAttackEffect.SetActive(false);
        animator.SetBool("Idle", true);
        RecoverHP();
        GameManager.instance.UpdateHP(hp, hpMax);
        dmgImage.SetActive(false);
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

            if(Input.GetAxisRaw("Horizontal")!=0||Input.GetAxisRaw("Vertical")!=0)
            {
                animator.SetBool("Idle", false);
                animator.SetBool("Walk", true);
            }
            else
            {
                animator.SetBool("Idle", true);
                animator.SetBool("Walk", false);
            }
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
                animator.SetTrigger("Attack");

                switch (selectedWeaponNum)
                {                    
                    case 0:                        
                        //GameObject Arrow = Instantiate(arrow, bowFirePoint.position, bowFirePoint.rotation) as GameObject;
                        //Destroy(Arrow.gameObject, 10);
                        break;
                    case 1:
                        //spearAttackEffect.gameObject.SetActive(true);
                        //StartCoroutine(WaitAndHide(spearAttackEffect));
                        //GameObject SpearAttackEffect = Instantiate(spearAttackEffect, spearAttackPos.position, spearAttackPos.rotation) as GameObject;
                        //Destroy(SpearAttackEffect, 0.5f);
                        break;
                    case 2:
                        //GameObject SwordAttackEffect = Instantiate(swordAttackEffect, swordAttackPos.position, //swordAttackPos.rotation) as GameObject;
                        //SwordAttackEffect.transform.SetParent(swordAttackPos);
                        //Destroy(SwordAttackEffect, 0.5f);
                        break;
                }
            }
            
            
        }

        if(Input.GetKeyUp(KeyCode.Alpha1))
        {
            WeaponSelect(0);
        }
        else if(Input.GetKeyUp(KeyCode.Alpha2))
        {
            WeaponSelect(1);
        }
        else if(Input.GetKeyUp(KeyCode.Alpha3))
        {
            WeaponSelect(2);
        }

    }
    /*
    IEnumerator WaitAndHide(GameObject name)
    {
        yield return new WaitForSeconds(0.5f);
        name.SetActive(false);
    }
    */
    public void BowAttack()
    {
        GameObject Arrow = Instantiate(arrow, bowFirePoint.position, bowFirePoint.rotation) as GameObject;
        Arrow.GetComponent<Arrow>().attackPower += GameManager.instance.attackPower[GameManager.instance.lv];
        Destroy(Arrow.gameObject, 10);
    }

    public void SwordAttack()
    {
        GameObject SwordAttackEffect = Instantiate(swordAttackEffect, swordAttackPos.position, swordAttackPos.rotation) as GameObject;
        SwordAttackEffect.GetComponent<AttackCheck>().attackPower += GameManager.instance.attackPower[GameManager.instance.lv];
        SwordAttackEffect.transform.SetParent(swordAttackPos);
        Destroy(SwordAttackEffect, 0.5f);
    }

    public void SpearAttack()
    {
        GameObject SpearAttackEffect = Instantiate(spearAttackEffect, spearAttackPos.position, spearAttackPos.rotation) as GameObject;
        SpearAttackEffect.GetComponent<AttackCheck>().attackPower += GameManager.instance.attackPower[GameManager.instance.lv];
        SpearAttackEffect.transform.SetParent(spearAttackPos);
        Destroy(SpearAttackEffect, 0.5f);
    }

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

        switch(num)
        {
            case 0:
                print("활 선택");
                animator.SetBool("Bow", true);
                animator.SetBool("Sword", false);
                //animator.SetBool("Spear", false);
                break;
            case 1:
                print("창 선택");
                animator.SetBool("Bow", false);
                animator.SetBool("Sword", false);                
                break;
            case 2:
                print("칼 선택");
                animator.SetBool("Bow", false);
                animator.SetBool("Sword", true);
                break;
        }
    }

    public void ComputeDamage(float dmg)
    {
        GameObject HitEffect = Instantiate(hitEffect, transform.position, Quaternion.identity) as GameObject;
        Destroy(HitEffect, 1.4f);
        //animator.SetTrigger("Damage");
        animator.Play("hit");

        dmgImage.SetActive(true);
        StartCoroutine(WaitAndHideImg());

        hp -= dmg;
        
        if (hp <= 0)
        {
            //사망 동작
            //animator.SetBool("Die", true);

            GameManager.instance.SaveBody(map.mapIndex, map.maps[map.mapIndex].seed, GameManager.instance.coin, transform.position);

            //GameObject Body=Instantiate(body, transform.position, Quaternion.identity)as GameObject;
            //Body.GetComponent<Body>().coin = GameManager.instance.coin;

            Reset();
        }

        GameManager.instance.UpdateHP(hp, hpMax);
    }

    IEnumerator WaitAndHideImg()
    {
        yield return new WaitForSeconds(1);
        dmgImage.SetActive(false);
    }

    public void Reset()
    {
        GameManager.instance.Reset();        
        map.mapIndex = 0;
        ResetForNextStage();
    }

    public void ResetForNextStage()
    {
        transform.position = new Vector3(0, 6.5f, 0);
        hp = hpMax;
        GameManager.instance.UpdateHP(hp, hpMax);
    }

    public void RecoverHP()
    {
        hp = hpMax;
    }
}
