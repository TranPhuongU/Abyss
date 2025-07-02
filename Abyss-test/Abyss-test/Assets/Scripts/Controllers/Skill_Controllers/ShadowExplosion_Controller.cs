using UnityEngine;

public class ShadowExplosion_Controller : MonoBehaviour
{
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsPlayer;

    [SerializeField] private Vector2 boxSize;
    [SerializeField] private Transform check;



    private BoxCollider2D bc;

    private Player player;
    private Enemy_Shadow enemy;


    private void Awake()
    {
        player = PlayerManager.instance.player;
        bc = GetComponent<BoxCollider2D>();
    }

    public void SetupExplosion(Enemy_Shadow _enemy)
    {
        enemy = _enemy;
    }

    private void Start()
    {
        float xOffset = 0;

        if(player.rb.linearVelocity.x != 0)
        {
            xOffset = player.facingDir * .6f;
        }

        transform.position = new Vector3(transform.position.x + xOffset, transform.position.y - GroundBelow().distance + (bc.size.y / 2));

    }

    private void Update()
    {
        
    }

    public void ShadowExplosionTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, bc.size,0, whatIsPlayer);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                player.SetupKnockbackPower(new Vector2(10, 6));
                PlayerStats targetStats = hit.GetComponent<PlayerStats>();

                enemy.stats.DoDamageRate(targetStats, 2);
            }
        }
    } 

    private RaycastHit2D GroundBelow() => Physics2D.Raycast(transform.position, Vector2.down, 100, whatIsGround);

    private void DestroyMe() => Destroy(gameObject);
    private void OnDrawGizmos() => Gizmos.DrawWireCube(check.position, boxSize);
}
