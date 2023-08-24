using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    public int coin;

    private void OnMouseUp()
    {
        GameManager.instance.UpdateCoin(coin);
        coin = 0;
        Destroy(this.gameObject, 2);
    }
}
