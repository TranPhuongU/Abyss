using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandBomb_Skill_Controller : MonoBehaviour
{
    private float damagePercent;
    private Animator anim;
    private Rigidbody2D rb;

    public Transform attackCheck;
    public float attackRadius;
    private float explodeCooldown;
    private float sandBombDuration;
    private CharacterStats playerStats;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        explodeCooldown = 0.1f;
    }

    private void Update()
    {
        explodeCooldown -= Time.deltaTime;
        sandBombDuration -= Time.deltaTime;

        if (sandBombDuration <= 0)
            anim.SetTrigger("Explode");
    }

    public void SetupSandBomb(CharacterStats _playerStats, float _damagePercent, float _sandBombDuration)
    {
        playerStats = _playerStats;
        damagePercent = _damagePercent;
        sandBombDuration = _sandBombDuration;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Enemy>() != null && explodeCooldown <= 0)
        {
            anim.SetTrigger("Explode");
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    private void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                EnemyStats targetStat = hit.GetComponent<EnemyStats>();
                playerStats.DoMagicalDamageRate(targetStat, damagePercent);

                ItemData_Equipment equipedAmulet = Inventory.instance.GetEquipment(EquipmentType.Amulet);

                if (equipedAmulet != null)
                    equipedAmulet.Effect(hit.transform);
            }
        }
    }

    private void selfDestroy() => Destroy(gameObject);

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackCheck.position, attackRadius);
    }
}
