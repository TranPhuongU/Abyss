using UnityEngine;

public class ShadowUltimateState : EnemyState
{
    Enemy_Shadow enemy;
    private int explosionAmount;

    public ShadowUltimateState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Shadow _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        explosionAmount = enemy.explosionAmount;

        enemy.stats.magicResistance.AddModifier(enemy.buffPercent);
        enemy.stats.armor.AddModifier(enemy.buffPercent);

        stateTimer = 1f;
    }

    public override void Exit()
    {
        base.Exit();

        enemy.stats.magicResistance.RemoveModifier(enemy.buffPercent);
        enemy.stats.armor.RemoveModifier(enemy.buffPercent);

    }

    public override void Update()
    {
        base.Update();

        if (stateTimer <= 0)
        {
            enemy.ExplosionTrigger();

            AudioManager.instance.PlaySFX2(46, enemy.transform);

            explosionAmount--;
            stateTimer = .6f;

            if (explosionAmount <= 0)
            {
                stateMachine.ChangeState(enemy.idleState);
            }

        }
    }
}
