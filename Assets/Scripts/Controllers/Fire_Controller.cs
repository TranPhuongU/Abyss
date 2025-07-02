using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Fire_Controller : MonoBehaviour
{
    public float burnRadius;
    public LayerMask whatIsPlayer;
    public float burnDelay;
    private float burnDamagePercent;

    private float burnTime;

    public float timer;

    private CharacterStats myStat;

    public void SetupFire(float _radius, LayerMask _whatisPlayer, float _cooldown, CharacterStats _stats, float _burnDamagePercent)
    {
        burnRadius = _radius;
        whatIsPlayer = _whatisPlayer;
        timer = _cooldown;
        burnDelay = _cooldown;
        myStat = _stats;
        burnDamagePercent = _burnDamagePercent;

    }

    private void Start()
    {
        timer = burnDelay;
        burnTime = 4;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        burnTime -= Time.deltaTime;

        if(timer <= 0)
        {
            Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, burnRadius, whatIsPlayer);
            foreach(var hit in collider)
            {
                if(hit.GetComponent<Player>() != null)
                {
                    PlayerStats target = hit.GetComponent<PlayerStats>();
                    ((EnemyStats)myStat).DoBurnDamageRate(target, burnDamagePercent);
                    timer = burnDelay;

                }
            }
        }

        if(burnTime <= 0)
        {
            Destroy(gameObject);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, burnRadius);
    }
}
