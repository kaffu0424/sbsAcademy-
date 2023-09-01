using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    public int coin;
    public GameObject bodyEffect;

    private void OnMouseUp()
    {
        GameManager.instance.UpdateCoin(coin);
        coin = 0;
        GameObject BodyEffect = Instantiate(bodyEffect, transform.position, Quaternion.identity) as GameObject;
        GameManager.instance.SaveBody(0, 999, 0, transform.position);
        Destroy(this.gameObject);
        Destroy(BodyEffect, 2);        
    }
}
