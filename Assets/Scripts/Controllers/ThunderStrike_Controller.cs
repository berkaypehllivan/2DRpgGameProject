using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStrike_Controller : MonoBehaviour
{

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            Player_Stats playerStats = PlayerManager.instance.player.GetComponent<Player_Stats>();
            Enemy_Stats enemyTarget = collision.GetComponent<Enemy_Stats>();
            playerStats.DoMagicalDamage(enemyTarget);
        }
    }
}
