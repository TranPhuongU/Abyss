using UnityEngine;

public class Spike_Controller : MonoBehaviour
{
    [SerializeField] private int damage = 10;

    [SerializeField] private float dmgCooldown;
    private float cooldownTimer;

    private void Start()
    {
        cooldownTimer = dmgCooldown;
    }
    private void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerStats>(out var playerStats) && cooldownTimer <= 0)
        {
            playerStats.TakeDamage(damage, Color.white);
            cooldownTimer = dmgCooldown;
        }
    }
}
