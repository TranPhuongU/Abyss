using UnityEngine;

public class LightOrb_Controller : MonoBehaviour
{
    private Rigidbody2D rb;

    private CharacterStats myStats;
    private float speed;
    private Vector2 dir;
    private float damagePercent;
    private float size;
    private float gravity;

    [SerializeField] private GameObject smokeFxPrefab;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        transform.localScale = new Vector3(size, size, size);
        rb.gravityScale = gravity;

        rb.linearVelocity = dir * speed;
    }

    public void SetupLightOrb(float _speed, CharacterStats _myStats, Vector2 _direction, float _damagePercent, float _size, float _gravity)
    {
        speed = _speed;
        dir = _direction;
        myStats = _myStats;
        damagePercent = _damagePercent;
        size = _size;
        gravity = _gravity;
    }

    private void Update()
    {
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            PlayerStats targetStats = collision.GetComponent<PlayerStats>();

            //collision.GetComponent<Player>().SetupKnockbackPower(new Vector2(10, 6));

            myStats.DoDamageRate(targetStats, damagePercent);
        }

        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Vector2 contactPoint = collision.ClosestPoint(transform.position);
            Instantiate(smokeFxPrefab, contactPoint, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
