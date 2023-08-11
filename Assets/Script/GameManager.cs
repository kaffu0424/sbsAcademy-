using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region ΩÃ±€≈Ê
    public static GameManager instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    public float moveSpeed;

    private void Start()
    {
        moveSpeed = 10f;
    }
}
