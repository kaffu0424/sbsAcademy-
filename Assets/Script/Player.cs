using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float moveSpeed , moveCount;
    public bool enableTurn;
    private void Start()
    {
        enableTurn = true;
        moveCount = 1;
        moveSpeed = GameManager.instance.moveSpeed;
    }
    private void Update()
    {
        if (enableTurn)
        {
            float moveAmt = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
            float moveAmt2 = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

            Vector3 moveVector = Vector3.right * moveAmt + Vector3.forward * moveAmt2;
            transform.Translate(moveVector);
        }
    }
}
