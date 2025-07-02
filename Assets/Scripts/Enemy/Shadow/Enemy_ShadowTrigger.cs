using UnityEngine;

public class Enemy_ShadowTrigger : Enemy_AnimationTrigger
{
    private Enemy_Shadow enemyShadow;

    protected override void Start()
    {
        base.Start();

        enemyShadow = GetComponentInParent<Enemy_Shadow>();
    }

    private void HorizontalFire() => enemyShadow.HorizontalAttackTrigger();

    protected virtual void BoomAttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemyShadow.boomPoint.position, enemyShadow.boomRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {

                PlayerStats target = hit.GetComponent<PlayerStats>();
                enemy.stats.DoDamageRate(target, enemyShadow.boomDMGPercent);
                enemy.hitDetected = true;
            }
        }
    }

    protected virtual void Attack4Trigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemyShadow.transform.position, enemy.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {

                PlayerStats target = hit.GetComponent<PlayerStats>();
                enemy.stats.DoDamage(target);
            }
        }
    }

    public void PunchTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                hit.GetComponent<Player>().SetupKnockbackPower(new Vector2(10,6));

                PlayerStats target = hit.GetComponent<PlayerStats>();
                enemy.stats.DoDamage(target);
            }
        }
    }

    private void MakeInvincible() => enemy.stats.MakeInvincible(true);
    private void FalseInvincible() => enemy.stats.MakeInvincible(false);
}
