using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrimsonBullet1_Controller : MonoBehaviour
{
    private SpriteRenderer sr;

    private float radiusCheck;
    private LayerMask whatIsPlayer;
    private float cooldown;
    private float burnDamagePercent;

    private float speed;
    private float bulletDamagePercent;
    private Vector2 direction;
    private GameObject firePrefab;
    private CharacterStats myStats;
    private Rigidbody2D rb;

    private bool canFx;
    public GameObject fxPrefab;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void SetupCrimsonBullet(float _speed, CharacterStats _myStats, Vector2 _direction, float _radius, LayerMask _whatIsPlayer, float _cooldown, GameObject _firePrefab, float _burnDamagePercent, float _bulletDamagePercent, bool _fx)
    {
        sr = GetComponent<SpriteRenderer>();
        speed = _speed;
        myStats = _myStats;
        direction = _direction;
        radiusCheck = _radius;
        whatIsPlayer = _whatIsPlayer;
        cooldown = _cooldown;
        firePrefab = _firePrefab;
        burnDamagePercent = _burnDamagePercent;
        bulletDamagePercent = _bulletDamagePercent;
        canFx = _fx;
    }
    private void Update()
    {
        rb.linearVelocity = direction * speed;

        transform.right = rb.linearVelocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            myStats.DoMagicalDamageRate(collision.GetComponent<CharacterStats>(), bulletDamagePercent);

        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if (!canFx)
            {
                Vector2 contactPoint = collision.ClosestPoint(transform.position);
                Instantiate(firePrefab, contactPoint, Quaternion.identity);
            }
            else
            {
                Vector2 contactPoint2 = collision.ClosestPoint(transform.position);
                Instantiate(fxPrefab, contactPoint2, Quaternion.identity);
            }

            
            Destroy(gameObject);
        }
    }
}
