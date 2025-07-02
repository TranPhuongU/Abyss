using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashWave_Controller : MonoBehaviour
{
    private SpriteRenderer sr;

    [SerializeField] GameObject smokeFxPrefab;

    private float damageRate;
    public float speedTimer;
    private float xVelocity;
    private Rigidbody2D rb;

    private CharacterStats myStats;
    private int facingDir = 1;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        speedTimer -= Time.deltaTime;
        
        if(speedTimer > 0)
            rb.linearVelocity = new Vector2(xVelocity - 9 * PlayerManager.instance.player.facingDir, rb.linearVelocity.y);
        else
            rb.linearVelocity = new Vector2(xVelocity, rb.linearVelocity.y);

        if (facingDir == 1 && rb.linearVelocity.x < 0)
        {
            facingDir = -1;
            sr.flipX = true;
        }
    }

    public void SetupSlashWave(float _speed, CharacterStats _myStats, float _damageRate)
    {
        sr = GetComponent<SpriteRenderer>();
        xVelocity = _speed;
        myStats = _myStats;
        damageRate = _damageRate;
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            EnemyStats _target = collision.GetComponent<EnemyStats>();

            myStats.DoDamageRate(_target, damageRate);
            
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Vector2 contactPoint = collision.ClosestPoint(transform.position);
            Instantiate(smokeFxPrefab, contactPoint, Quaternion.identity);
            Destroy(gameObject);
        }


    }
}
