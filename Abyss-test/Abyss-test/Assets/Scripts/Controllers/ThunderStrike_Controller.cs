using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStrike_Controller : MonoBehaviour
{
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Enemy>() != null)
        {
            PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
            Player player = PlayerManager.instance.player;
            EnemyStats enemyTarget = collision.GetComponent<EnemyStats>();

            if(player.isForm2)
                playerStats.DoMagicalDamageRate(enemyTarget, .1f);
            else
                playerStats.DoDamageRate(enemyTarget, .1f);
        }
    }
}
