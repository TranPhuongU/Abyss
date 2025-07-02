using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion_Controller : MonoBehaviour
{
    public Player player;

    private CharacterStats myStats;
    private CircleCollider2D cd;
    private float xVelocity;

    private float damageRate;

    private void Start()
    {
        cd = GetComponent<CircleCollider2D>();


    }

    public void SetupExplosion(CharacterStats _stats, float _damagePercent, float _xVelocity)
    {
        myStats = _stats;
        damageRate = _damagePercent;
        xVelocity = _xVelocity;

       if(xVelocity < 0)
        {
            transform.Rotate(0, 180, 0);

        }

    }

    private void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Entity>() != null)
            {
                myStats.DoMagicalDamageRate(hit.GetComponent<CharacterStats>(), damageRate);
            }
        }
    }

    private void DestroyMe() => Destroy(gameObject);
}
