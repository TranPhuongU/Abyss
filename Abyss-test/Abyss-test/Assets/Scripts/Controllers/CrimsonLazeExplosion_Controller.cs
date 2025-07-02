using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CrimsonLazeExplosion_Controller : MonoBehaviour
{
    private CircleCollider2D cd;
    private CharacterStats myStats;
    private float damageRate;
    private float xVelocity;

    public Transform wallCheck;
    public float wallCheckRadius;

    public LayerMask whatIsGround;

    public GameObject explosionPrefab;

    private Enemy enemy;

    private float facingDir;

    private void Start()
    {
        cd = GetComponent<CircleCollider2D>();
    }

    public void SetupExplosion(CharacterStats _stats, float _damagePercent, float _facingDir, Enemy _enemy)
    {
        myStats = _stats;
        damageRate = _damagePercent;

        enemy = _enemy;

        facingDir = _facingDir;

        if (facingDir == -1)
        {
            transform.Rotate(0, 180, 0);

        }

    }

    private void DestroyMe() => Destroy(gameObject);

    private void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                myStats.DoMagicalDamageRate(hit.GetComponent<CharacterStats>(), damageRate);
            }
        }
    }

    private bool WallCheck() => Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, whatIsGround);

    private void CreateExplosion()
    {
        if (!WallCheck())
        {
            GameObject newExplosion = Instantiate(explosionPrefab, new Vector3(transform.position.x +3 * facingDir, transform.position.y), Quaternion.identity);

            newExplosion.GetComponent<CrimsonLazeExplosion_Controller>().SetupExplosion(myStats, 1, facingDir, enemy);
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(wallCheck.position, wallCheckRadius);
    }

}
