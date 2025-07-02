using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DarkOrb_Controller : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("Explosion info")]
    private float damagePercent;

    [SerializeField] private GameObject explosionPrefab;

    [Header("Dark Orb info")]
    private float damage;
    [SerializeField] private string targetLayerName = "Player";

    [SerializeField] private float xVelocity;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private bool flipped;

    private CharacterStats myStats;

    private void Update()
    {
          rb.linearVelocity = new Vector2(xVelocity, rb.linearVelocity.y);

    }

    public void SetupDarkOrb(float _speed, CharacterStats _myStats, float _darkOrbDamagePercent, float _explosionDamagePercent)
    {
        sr = GetComponent<SpriteRenderer>();
        xVelocity = _speed;
        myStats = _myStats;
        damagePercent = _explosionDamagePercent;
        damage = _darkOrbDamagePercent;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(targetLayerName))
        {
            myStats.DoMagicalDamageRate(collision.GetComponent<CharacterStats>(), damage);

            if (PlayerManager.instance.player.dashing)
                return;

            GameObject newExplosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            newExplosion.GetComponent<Explosion_Controller>().SetupExplosion(myStats,damagePercent, rb.linearVelocity.x);

        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            GameObject newExplosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            newExplosion.GetComponent<Explosion_Controller>().SetupExplosion(myStats, damagePercent, rb.linearVelocity.x);

            Destroy(gameObject);
        }
    }
   
    public void FlipDarkOrb()
    {
        if (flipped)
            return;

        xVelocity *= -1;
        flipped = true;
        transform.Rotate(0, 180, 0);
        targetLayerName = "Enemy";
    }

}
