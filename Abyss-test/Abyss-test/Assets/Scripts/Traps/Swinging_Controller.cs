using UnityEngine;

public class Swinging_Controller : MonoBehaviour
{
    [SerializeField] private int damage = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerStats>(out var playerStats))
        {
            collision.GetComponent<Player>().SetupKnockbackDir(transform);
            collision.GetComponent<Player>().SetupKnockbackPower(new Vector2(20, 6));



            playerStats.TakeDamage(damage, Color.white);


        }
    }
}
