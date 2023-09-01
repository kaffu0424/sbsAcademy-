using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAction : MonoBehaviour
{
    public Enemy enemy;

    public void DoAttack()
    {
        enemy.DoAttack();
    }
}
