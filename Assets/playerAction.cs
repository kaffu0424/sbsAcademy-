using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAction : MonoBehaviour
{
    public Player player;

    public void BowAttack()
    {
        player.BowAttack();
    }

    public void SwordAttack()
    {
        player.SwordAttack();
    }

    public void SpearAttack()
    {
        player.SpearAttack();
    }
}
